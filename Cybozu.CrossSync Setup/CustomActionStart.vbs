Set WshShell = CreateObject("WScript.Shell")
WshShell.Run """" & Property("CustomActionData") & """",4,False
Set WshShell = Nothing
