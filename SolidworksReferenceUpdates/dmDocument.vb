Imports System.Collections.ObjectModel
Imports SwDocumentMgr
Imports PropertyChanged

<ImplementPropertyChanged>
Public Class dmDocument
    Public Shared dmDocMgr As SwDMApplication4 = GetSwDocMgr()

    Public Shared AllFiles As New Dictionary(Of String, dmDocument)

    Public Property Children As New ObservableCollection(Of dmDocument)

    Private _FileName As String

    Public Property FullFileName As String

    Public Property SelectedConfiguration As String
    Public Property IsValid As Boolean = True
    Public Property HasDrawing As String = "N/A"

    Public Property Level As Integer
    Public ReadOnly Property FileExists As Boolean
        Get
            Return IO.File.Exists(FullFileName)
        End Get
    End Property

#Region "For Treeview"
    Public Property IsSelected As Boolean
    Public Property IsExpanded As Boolean = True
#End Region
    Public Property FileName As String
        Get
            Return _FileName
        End Get
        Set(value As String)
            _FileName = IO.Path.GetFileNameWithoutExtension(value).ToUpper
        End Set
    End Property
    Public Property FileNameExt As String
        Get
            Return _FileName
        End Get
        Set(value As String)
            _FileName = IO.Path.GetFileName(value).ToUpper
        End Set
    End Property

    Sub New(FilePath_Name As String, config As String)
        SelectedConfiguration = config
        FullFileName = FilePath_Name
        FileName = FilePath_Name
        FileNameExt = FilePath_Name

        Dim Key As String = FileName & "-" & SelectedConfiguration

        If IO.File.Exists(IO.Path.ChangeExtension(FullFileName, ".slddrw")) Then
            HasDrawing = True.ToString
        Else
            HasDrawing = False.ToString
        End If

        Try
            AllFiles.Add(Key, Me)
        Catch
            IsValid = False
        End Try
    End Sub

    Public Sub GetReferences()
        If Not IsValid Then Exit Sub
        If Not IO.File.Exists(FullFileName) Then Exit Sub
        Dim dmDoc As ISwDMDocument18 = OpenDocument(FullFileName)
        If dmDoc Is Nothing Then Exit Sub
        Try
            'used when file can not be found
            Dim dmExtRefOption As SwDMExternalReferenceOption2
            Dim swSearchOpt As SwDMSearchOption = dmDocMgr.GetSearchOptionObject
            dmExtRefOption = dmDocMgr.GetExternalReferenceOptionObject2
            dmExtRefOption.NeedSuppress = True
            dmExtRefOption.SearchOption = swSearchOpt

            Dim numExtRefs As Integer = dmDoc.GetExternalFeatureReferences2(dmExtRefOption)
            Dim extRefs() As String = dmExtRefOption.ExternalReferences
            'Dim refConfigs() As String = dmExtRefOption.ReferencedConfigurations
            'Dim brokenStatus() As Integer = dmExtRefOption.BrokenStatus
            'Dim refIDs() As Integer = dmExtRefOption.IDs


            Dim configMgr As SwDMConfigurationMgr = dmDoc.ConfigurationManager

            Dim config As SwDMConfiguration12 = configMgr.GetConfigurationByName(SelectedConfiguration)
            Dim comps As Object = config.GetComponents()

            If comps Is Nothing Then Exit Sub
            For Each swComp As SwDMComponent9 In comps
                If Not swComp.ExcludeFromBOM AndAlso Not swComp.IsSuppressed Then
                    Dim UsedConfig As String = swComp.ConfigurationName

                    Dim RefFileName As String = swComp.PathName

                    Try                   'Check updated paths
                        'If that file cannot be found, look for an external reference with the same name
                        RefFileName = FindExtRefPath(swComp.PathName, extRefs)
                    Catch
                    End Try

                    Dim Key As String = IO.Path.GetFileNameWithoutExtension(RefFileName).ToUpper & "-" & UsedConfig
                    If AllFiles.ContainsKey(Key) Then
                        Children.Add(AllFiles.Item(Key))
                    Else
                        Dim doc As New dmDocument(RefFileName, UsedConfig)
                        Children.Add(doc)
                        doc.GetReferences()
                    End If
                End If
            Next

        Finally
            dmDoc.CloseDoc()
        End Try
    End Sub
    Private Function FindExtRefPath(ByVal name As String, ByVal arrComps As Array) As String
        Dim ExtrefPathName As String = name
        Dim nameParts As String() = name.Split("\"c)
        Dim justName As String = nameParts.GetValue(nameParts.GetUpperBound(0)).ToString()

        Dim i As Integer = arrComps.GetLowerBound(0)
        Dim found As [Boolean] = False

        While (i <= arrComps.GetUpperBound(0)) AndAlso Not found
            Dim extref As String = (arrComps.GetValue(i)).ToString()
            Dim extrefParts As String() = extref.Split("\"c)
            Dim justextrefName As String = extrefParts.GetValue(extrefParts.GetUpperBound(0)).ToString()
            If justextrefName = justName Then
                found = True
                ExtrefPathName = extref
            End If
            i += 1
        End While
        Return ExtrefPathName
    End Function

    Public Shared Function OpenDocument(FullFileName As String) As SwDMDocument18
        Dim Errormsg As SwDmDocumentOpenError = SwDmDocumentOpenError.swDmDocumentOpenErrorNone 'swFileLoadError_e
        Dim Type As SwDmDocumentType = GetDocumentType(FullFileName)
        Dim dmDoc As SwDMDocument18 = dmDocMgr.GetDocument(FullFileName, Type, True, Errormsg)
        If dmDoc IsNot Nothing AndAlso Errormsg = SwDmDocumentOpenError.swDmDocumentOpenErrorNone Then
            Return dmDoc
        Else
            Return Nothing
        End If
    End Function
    Public Shared Function GetDocumentType(FullFileName As String) As SwDmDocumentType
        Dim Type As SwDmDocumentType = SwDmDocumentType.swDmDocumentUnknown
        Select Case IO.Path.GetExtension(FullFileName).ToLower
            Case ".sldprt"
                Type = SwDmDocumentType.swDmDocumentPart
            Case ".sldasm"
                Type = SwDmDocumentType.swDmDocumentAssembly
            Case ".slddraw"
                Type = SwDmDocumentType.swDmDocumentDrawing
        End Select
        Return Type
    End Function
End Class
