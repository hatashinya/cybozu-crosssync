using System;
using System.Xml;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CBLabs.CybozuConnect
{
    public enum ScheduleEventType
    {
        Normal,
        Banner,
        Repeat,
        Temporary
    }

    public enum SchedulePublicType
    {
        Public,
        Private,
        Qualified
    }

    public class ScheduleEvent
    {
        public string ID;
        public string Version;
        public ScheduleEventType EventType = ScheduleEventType.Normal;
        public SchedulePublicType PublicType = SchedulePublicType.Public;
        public string StartValue;
        public string EndValue;
        public DateTime Start = DateTime.MinValue;
        public DateTime End = DateTime.MinValue;
        public bool AllDay = false;
        public bool StartOnly = false;
        public string Plan;
        public string Detail;
        public string Description;
        public StringCollection UserIds = new StringCollection();
        public StringCollection OrganizaitonIds = new StringCollection();
        public StringCollection FacilityIds = new StringCollection();

        public ScheduleEvent()
        {
        }

        public ScheduleEvent(XmlNode eventNode)
        {
            this.ID = Utility.SafeAttribute(eventNode, "id");
            this.Version = Utility.SafeAttribute(eventNode, "version");

            //string eventType = Utility.Capitalize(Utility.SafeAttribute(eventNode, "event_type"));
            //if (!Enum.TryParse<ScheduleEventType>(eventType, out this.EventType))
            //{
            //    this.EventType = ScheduleEventType.Normal;
            //}
            try
            {
                this.EventType = (ScheduleEventType)Enum.Parse(typeof(ScheduleEventType), Utility.SafeAttribute(eventNode, "event_type"), true);
            }
            catch (Exception)
            {
                this.EventType = ScheduleEventType.Normal;
            }

            //string publicType = Utility.Capitalize(Utility.SafeAttribute(eventNode, "public_type"));
            //if (!Enum.TryParse<SchedulePublicType>(publicType, out this.PublicType))
            //{
            //    this.PublicType = SchedulePublicType.Public;
            //}
            try
            {
                this.PublicType = (SchedulePublicType)Enum.Parse(typeof(SchedulePublicType), Utility.SafeAttribute(eventNode, "public_type"), true);
            }
            catch (Exception)
            {
                this.PublicType = SchedulePublicType.Public;
            }

            this.AllDay = (Utility.SafeAttribute(eventNode, "allday").ToLowerInvariant() == bool.TrueString.ToLowerInvariant());
            this.StartOnly = (Utility.SafeAttribute(eventNode, "start_only").ToLowerInvariant() == bool.TrueString.ToLowerInvariant());

            // when/date|datetime
            XmlNode whenNode = eventNode["when"];
            if (whenNode != null)
            {
                XmlNode dateNode = whenNode["date"];
                XmlNode dateTimeNode = whenNode["datetime"];
                if (dateNode != null && (this.AllDay || this.EventType == ScheduleEventType.Banner))
                {
                    this.StartValue = Utility.SafeAttribute(dateNode, "start");
                    this.Start = Utility.ParseXSDDate(this.StartValue);
                    if (!this.StartOnly)
                    {
                        this.EndValue = Utility.SafeAttribute(dateNode, "end");
                        this.End = Utility.ParseXSDDate(this.EndValue);
                    }
                }
                else if (dateTimeNode != null && !this.AllDay && this.EventType != ScheduleEventType.Banner)
                {
                    string startValue = Utility.SafeAttribute(dateTimeNode, "start");
                    this.Start = Utility.ParseISO8601(startValue, true).ToLocalTime();
                    this.StartValue = Utility.FormatXSDDateTime(this.Start);
                    if (!this.StartOnly)
                    {
                        string endValue = Utility.SafeAttribute(dateTimeNode, "end");
                        this.End = Utility.ParseISO8601(endValue, true).ToLocalTime();
                        this.EndValue = Utility.FormatXSDDateTime(this.End);
                    }
                }
            }

            this.Plan = Utility.SafeAttribute(eventNode, "plan");
            this.Detail = Utility.SafeAttribute(eventNode, "detail");
            this.Description = Utility.SafeAttribute(eventNode, "description");

            // members
            XmlNode membersNode = eventNode["members"];
            if (membersNode != null)
            {
                foreach (XmlNode memberNode in membersNode.ChildNodes)
                {
                    if (memberNode.Name != "member") continue;
                    if (memberNode.FirstChild == null) continue;

                    string id = Utility.SafeAttribute(memberNode.FirstChild, "id");
                    if (string.IsNullOrEmpty(id)) continue;
                    switch (memberNode.FirstChild.Name)
                    {
                        case "user":
                            this.UserIds.Add(id);
                            break;

                        case "organization":
                            this.OrganizaitonIds.Add(id);
                            break;

                        case "facility":
                            this.FacilityIds.Add(id);
                            break;
                    }
                }
            }
        }

        public bool IsNormal { get { return this.EventType == ScheduleEventType.Normal; } }
        public bool IsRepeat { get { return this.EventType == ScheduleEventType.Repeat; } }
        public bool IsTemporary { get { return this.EventType == ScheduleEventType.Temporary; } }
        public bool IsBanner { get { return this.EventType == ScheduleEventType.Banner; } }

        public bool IsPublic { get { return this.PublicType == SchedulePublicType.Public; } }
        public bool IsPrivate { get { return this.PublicType == SchedulePublicType.Private; } }
        public bool IsQualified { get { return this.PublicType == SchedulePublicType.Qualified; } }

        public int MemberCount { get { return this.UserIds.Count + this.OrganizaitonIds.Count + this.FacilityIds.Count; } }
    }

    public class ScheduleEventCollection : Collection<ScheduleEvent>
    {
    }

    public class Schedule
    {
        public readonly App App;
        public readonly Base Base;

        public enum TargetType { User, Organization, Facility };

        public Schedule(App app)
        {
            this.App = app;
            this.App.Schedule = this;
            this.Base = this.App.Base;
            if (this.Base == null)
            {
                this.Base = new Base(app);   
            }
        }

        public ScheduleEventCollection GetEventsByTarget(DateTime start, DateTime end, TargetType targetType, string targetId)
        {
            if (string.IsNullOrEmpty(targetId))
            {
                throw new CybozuException("Target ID is not specified.");
            }

            ListDictionary parameters = new ListDictionary();
            parameters["start"] = Utility.FormatXSDDateTime(start);
            parameters["end"] = Utility.FormatXSDDateTime(end);
            ListDictionary idParam = new ListDictionary();
            idParam["id"] = targetId;
            parameters[targetType.ToString().ToLowerInvariant()] = idParam;

            XmlElement resultNode = this.App.Query("Schedule", "ScheduleGetEventsByTarget", parameters);
            XmlNodeList eventNodeList = resultNode.SelectNodes("//schedule_event");

            ScheduleEventCollection eventList = new ScheduleEventCollection();
            foreach (XmlNode eventNode in eventNodeList)
            {
                try
                {
                    ScheduleEvent scheduleEvent = new ScheduleEvent(eventNode);
                    eventList.Add(scheduleEvent);
                }
                catch (Exception)
                {
                }
            }

            return eventList;
        }

        public ScheduleEvent AddEvent(ScheduleEvent scheduleEvent)
        {
            if (scheduleEvent.EventType != ScheduleEventType.Normal && scheduleEvent.EventType != ScheduleEventType.Banner)
            {
                throw new CybozuException("Cannot add the event of the specified type.");
            }

            if (scheduleEvent.MemberCount == 0)
            {
                throw new CybozuException("No participants.");
            }

            ListDictionary parameters = new ListDictionary();
            parameters["schedule_event"] = CreateScheduleEventParam(scheduleEvent, false);

            XmlElement result = this.App.Exec("Schedule", "ScheduleAddEvents", parameters);
            return new ScheduleEvent(result.SelectSingleNode("//schedule_event"));
        }

        public ScheduleEventCollection AddEvents(ScheduleEventCollection scheduleEvents)
        {
            ListDictionary parameters = new ListDictionary();
            ArrayList schedule_event_list = new ArrayList();
            foreach (ScheduleEvent scheduleEvent in scheduleEvents)
            {
                ListDictionary schedule_event = CreateScheduleEventParam(scheduleEvent, false);
                if (schedule_event == null) continue;
                schedule_event_list.Add(schedule_event);
            }
            parameters["schedule_event"] = schedule_event_list;

            XmlElement result = this.App.Exec("Schedule", "ScheduleAddEvents", parameters);
            XmlNodeList eventNodeList = result.SelectNodes("//schedule_event");

            ScheduleEventCollection eventList = new ScheduleEventCollection();
            foreach (XmlNode eventNode in eventNodeList)
            {
                try
                {
                    ScheduleEvent scheduleEvent = new ScheduleEvent(eventNode);
                    eventList.Add(scheduleEvent);
                }
                catch (Exception)
                {
                }
            }

            return eventList;
        }

        public void ModifyEvent(ScheduleEvent scheduleEvent)
        {
            if (scheduleEvent.EventType != ScheduleEventType.Normal && scheduleEvent.EventType != ScheduleEventType.Banner)
            {
                throw new CybozuException("Cannot modify the event of the specified type.");
            }

            if (scheduleEvent.MemberCount == 0)
            {
                throw new CybozuException("No participants.");
            }

            ListDictionary parameters = new ListDictionary();
            ListDictionary schedule_event = new ListDictionary();
            parameters["schedule_event"] = CreateScheduleEventParam(scheduleEvent, true);

            XmlElement result = this.App.Exec("Schedule", "ScheduleModifyEvents", parameters);

            if (result.SelectSingleNode("//schedule_event") == null)
            {
                throw new CybozuException("Fail to modify the specified event.");
            }
        }

        protected ListDictionary CreateScheduleEventParam(ScheduleEvent scheduleEvent, bool modify)
        {
            if (scheduleEvent.EventType != ScheduleEventType.Normal && scheduleEvent.EventType != ScheduleEventType.Banner) return null;
            if (scheduleEvent.MemberCount == 0) return null;

            ListDictionary schedule_event = new ListDictionary();
            schedule_event["xmlns"] = "";
            schedule_event["id"] = modify ? scheduleEvent.ID : "dummy";
            schedule_event["event_type"] = scheduleEvent.EventType.ToString().ToLowerInvariant();
            schedule_event["version"] = modify ? scheduleEvent.Version : "dummy";
            schedule_event["public_type"] = scheduleEvent.PublicType.ToString().ToLowerInvariant();
            schedule_event["plan"] = scheduleEvent.Plan;
            schedule_event["detail"] = scheduleEvent.Detail;
            schedule_event["description"] = scheduleEvent.Description;
            schedule_event["allday"] = scheduleEvent.AllDay;
            schedule_event["start_only"] = scheduleEvent.StartOnly;

            schedule_event["members"] = PrepareMembers(scheduleEvent);

            schedule_event["when"] = CreateWhen(scheduleEvent);

            return schedule_event;
        }

        public void RemoveEvent(string eventId)
        {
            ListDictionary parameters = new ListDictionary();
            ListDictionary event_id = new ListDictionary();
            event_id["innerValue"] = eventId;
            parameters["event_id"] = event_id;
            this.App.Exec("Schedule", "ScheduleRemoveEvents", parameters);
        }

        public void RemoveEvents(StringCollection eventIdList)
        {
            ListDictionary parameters = new ListDictionary();
            ArrayList event_id_list = new ArrayList();
            foreach (string eventId in eventIdList)
            {
                ListDictionary event_id = new ListDictionary();
                event_id["innerValue"] = eventId;
                event_id_list.Add(event_id);
            }
            parameters["event_id"] = event_id_list;
            this.App.Exec("Schedule", "ScheduleRemoveEvents", parameters);
        }

        public void LeaveEvent(string eventId)
        {
            ListDictionary parameters = new ListDictionary();
            ListDictionary event_id = new ListDictionary();
            event_id["innerValue"] = eventId;
            parameters["event_id"] = eventId;
            this.App.Exec("Schedule", "ScheduleLeaveEvents", parameters);
        }

        protected ListDictionary PrepareMembers(ScheduleEvent scheduleEvent)
        {
            ArrayList memberList = new ArrayList();

            foreach (string userId in scheduleEvent.UserIds)
            {
                ListDictionary user = new ListDictionary();
                user["id"] = userId;
                ListDictionary member = new ListDictionary();
                member["user"] = user;
                memberList.Add(member);
            }

            foreach (string orgId in scheduleEvent.OrganizaitonIds)
            {
                ListDictionary org = new ListDictionary();
                org["id"] = orgId;
                ListDictionary member = new ListDictionary();
                member["organization"] = org;
                memberList.Add(member);
            }

            foreach (string facilityId in scheduleEvent.FacilityIds)
            {
                ListDictionary facility = new ListDictionary();
                facility["id"] = facilityId;
                ListDictionary member = new ListDictionary();
                member["facility"] = facility;
                memberList.Add(member);
            }

            ListDictionary members = new ListDictionary();
            members["member"] = memberList;
            return members;
        }

        protected ListDictionary CreateWhen(ScheduleEvent scheduleEvent)
        {
            ListDictionary when = new ListDictionary();
            if (scheduleEvent.AllDay || scheduleEvent.EventType == ScheduleEventType.Banner)
            {
                ListDictionary date = new ListDictionary();
                date["start"] = Utility.FormatXSDDate(scheduleEvent.Start);
                if (!scheduleEvent.StartOnly)
                {
                    date["end"] = Utility.FormatXSDDate(scheduleEvent.End);
                }
                when["date"] = date;
            }
            else
            {
                ListDictionary datetime = new ListDictionary();
                datetime["start"] = Utility.FormatISO8601(scheduleEvent.Start.ToUniversalTime());
                if (!scheduleEvent.StartOnly)
                {
                    datetime["end"] = Utility.FormatISO8601(scheduleEvent.End.ToUniversalTime());
                }
                when["datetime"] = datetime;
            }
            return when;
        }
    }
}
