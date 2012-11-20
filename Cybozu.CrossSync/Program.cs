using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Text;

using CBLabs.CybozuConnect;

namespace Cybozu.CrossSync
{
    static class Program
    {
        public const string DescriptionHeaderName = "# CrossSync: "; // Don't modify this.

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SettingForm());
        }

        public static bool IsConfigured(Properties.Settings settings)
        {
            if (string.IsNullOrEmpty(settings.FirstUrl) || string.IsNullOrEmpty(settings.SecondUrl)) return false;
            if (string.IsNullOrEmpty(settings.FirstPostfix) || string.IsNullOrEmpty(settings.SecondPostfix)) return false;
            
            return true;
        }

        public static bool CanSync(out CybozuException ex)
        {
            ex = null;

            Properties.Settings settings = Properties.Settings.Default;
            if (!IsConfigured(settings)) return false;

            App firstApp, secondApp;
            Schedule firstSchedule, secondSchedule;

            try
            {
                firstApp = new App(settings.FirstUrl);
                firstApp.Auth(settings.FirstUsername, settings.FirstPassword);
                firstSchedule = new Schedule(firstApp);

                secondApp = new App(settings.SecondUrl);
                secondApp.Auth(settings.SecondUsername, settings.SecondPassword);
                secondSchedule = new Schedule(secondApp);
            }
            catch (CybozuException e)
            {
                // fail to auth
                ex = e;
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool Sync()
        {
            Properties.Settings settings = Properties.Settings.Default;
            if (!IsConfigured(settings)) return false;

            App firstApp = new App(settings.FirstUrl);
            firstApp.Auth(settings.FirstUsername, settings.FirstPassword);
            Schedule firstSchedule = new Schedule(firstApp);

            App secondApp = new App(settings.SecondUrl);
            secondApp.Auth(settings.SecondUsername, settings.SecondPassword);
            Schedule secondSchedule = new Schedule(secondApp);

            // sync span
            DateTime start = DateTime.Now.Date;
            DateTime end = start.AddMonths(1);

            // current events in first
            ScheduleEventCollection event1to2 = new ScheduleEventCollection();
            ScheduleEventCollection event1from2 = new ScheduleEventCollection();
            GetEvents(firstApp, firstSchedule, settings.SecondPostfix, start, end, event1to2, event1from2);

            // current events in second
            ScheduleEventCollection event2to1 = new ScheduleEventCollection();
            ScheduleEventCollection event2from1 = new ScheduleEventCollection();
            GetEvents(secondApp, secondSchedule, settings.FirstPostfix, start, end, event2to1, event2from1);

            // remove not modified
            UnsetNotModified(event1to2, event2to1, event2from1, settings.FirstPostfix);
            UnsetNotModified(event2to1, event1to2, event1from2, settings.SecondPostfix);

            // remove old copied events
            RemoveInvalidCopiedEvents(secondSchedule, event2from1);
            RemoveInvalidCopiedEvents(firstSchedule, event1from2);

            // add new copied events
            CopyValidEvents(secondSchedule, event1to2, settings.FirstPostfix);
            CopyValidEvents(firstSchedule, event2to1, settings.SecondPostfix);

            settings.LastSynchronized = DateTime.Now.ToString("o");
            settings.Save();

            return true;
        }

        public static void GetEvents(App app, Schedule schedule, string postfix, DateTime start, DateTime end, ScheduleEventCollection eventTo, ScheduleEventCollection eventFrom)
        {
            DateTime marginStart = start.AddDays(-1.0);
            DateTime marginEnd = end.AddDays(1.0);
            ScheduleEventCollection eventList = schedule.GetEventsByTarget(marginStart, marginEnd, Schedule.TargetType.User, app.UserId);

            Properties.Settings settings = Properties.Settings.Default;

            foreach (ScheduleEvent scheduleEvent in eventList)
            {
                if (scheduleEvent.StartOnly)
                {
                    if (scheduleEvent.Start.CompareTo(start) < 0) continue;
                }
                else if (scheduleEvent.AllDay || scheduleEvent.IsBanner)
                {
                    if (scheduleEvent.End.CompareTo(start) < 0) continue;
                }
                else
                {
                    if (scheduleEvent.Start.Equals(scheduleEvent.End))
                    {
                        if (scheduleEvent.End.CompareTo(start) < 0) continue;
                    }
                    else
                    {
                        if (scheduleEvent.End.CompareTo(start) <= 0) continue;
                    }
                }
                if (scheduleEvent.Start.CompareTo(end) >= 0) continue;

                if (scheduleEvent.Detail.EndsWith(postfix) || scheduleEvent.Description.StartsWith(DescriptionHeaderName))
                {
                    //if (scheduleEvent.MemberCount <= 1)
                    {
                        eventFrom.Add(scheduleEvent);
                    }
                }
                else if (!scheduleEvent.AllDay || (settings.AllDay && scheduleEvent.AllDay) || (settings.Banner && scheduleEvent.IsBanner) || (settings.Temporary && scheduleEvent.IsTemporary) || (settings.Private && scheduleEvent.IsPrivate) || (settings.Qualified && scheduleEvent.IsQualified))
                {
                    eventTo.Add(scheduleEvent);
                }
            }
        }

        public static void UnsetNotModified(ScheduleEventCollection srcEventList, ScheduleEventCollection origEventList, ScheduleEventCollection destEventList, string postfix)
        {
            for (int i = srcEventList.Count - 1; i >= 0; i--)
            {
                ScheduleEvent srcEvent = srcEventList[i];

                bool fromDest = false;
                ScheduleEvent destEvent = FindPairdEvent(srcEvent, origEventList, string.Empty);
                if (destEvent == null)
                {
                    destEvent = FindPairdEvent(srcEvent, destEventList, postfix);
                    fromDest = true;
                }
                if (destEvent == null) continue;

                srcEventList.Remove(srcEvent);
                if (fromDest)
                {
                    destEventList.Remove(destEvent);
                }
                else
                {
                    origEventList.Remove(destEvent);
                }
            }
        }

        public static void RemoveInvalidCopiedEvents(Schedule schedule, ScheduleEventCollection eventList)
        {
            if (eventList.Count == 0) return;

            StringCollection idList = new StringCollection();
            foreach (ScheduleEvent destEvent in eventList)
            {
                if (!string.IsNullOrEmpty(destEvent.ID))
                {
                    idList.Add(destEvent.ID);
                }
            }

            if (idList.Count == 0) return;

            schedule.RemoveEvents(idList);
        }

        public static void CopyValidEvents(Schedule schedule, ScheduleEventCollection eventList, string postfix)
        {
            if (eventList.Count == 0) return;

            ScheduleEventCollection newEventsList = new ScheduleEventCollection();
            foreach (ScheduleEvent srcEvent in eventList)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DescriptionHeaderName);
                sb.AppendLine(Resources.CrossSyncDescription);
                if (!string.IsNullOrEmpty(srcEvent.Description))
                {
                    sb.AppendLine();
                    sb.Append(srcEvent.Description);
                }

                ScheduleEvent newEvent = new ScheduleEvent();
                newEvent.EventType = srcEvent.IsBanner ? ScheduleEventType.Banner : ScheduleEventType.Normal;
                newEvent.PublicType = srcEvent.IsPublic ? SchedulePublicType.Public : SchedulePublicType.Private;
                newEvent.Start = srcEvent.Start;
                newEvent.End = srcEvent.End;
                newEvent.AllDay = srcEvent.AllDay;
                newEvent.StartOnly = srcEvent.StartOnly;
                newEvent.Plan = srcEvent.Plan;
                newEvent.Detail = srcEvent.Detail + postfix;
                newEvent.Description = sb.ToString();
                newEvent.UserIds.Add(schedule.App.UserId);

                newEventsList.Add(newEvent);
            }

            if (newEventsList.Count == 0) return;

            schedule.AddEvents(newEventsList);
        }

        public static ScheduleEvent FindPairdEvent(ScheduleEvent srcEvent, ScheduleEventCollection eventList, string postfix)
        {
            string detail = srcEvent.Detail + postfix;

            foreach (ScheduleEvent scheduleEvent in eventList)
            {
                // compare event type
                if ((srcEvent.IsBanner && !scheduleEvent.IsBanner) || (!srcEvent.IsBanner && scheduleEvent.IsBanner)) continue;

                // compare start date and time
                if (!srcEvent.Start.Equals(scheduleEvent.Start)) continue;

                // compare start only flag
                if (srcEvent.StartOnly)
                {
                    if (!scheduleEvent.StartOnly) continue;
                }
                else
                {
                    if (scheduleEvent.StartOnly) continue;

                    // compare end date and time
                    if (!srcEvent.End.Equals(scheduleEvent.End)) continue;
                }

                // compare plan
                if (srcEvent.Plan != scheduleEvent.Plan) continue;

                // compare detail
                if (detail == scheduleEvent.Detail) return scheduleEvent;
            }

            return null;
        }
    }
}
