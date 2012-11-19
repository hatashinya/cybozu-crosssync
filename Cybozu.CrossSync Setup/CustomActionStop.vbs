Set WMI = GetObject("winmgmts:")
For Each process in WMI.InstancesOf("Win32_process")
    If process.Name = Property("CustomActionData") Then
        process.Terminate
    End If
Next
Set WMI = Nothing
