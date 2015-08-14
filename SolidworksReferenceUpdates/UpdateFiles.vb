Imports System.Collections.ObjectModel
Imports SolidworksReferences
Imports SwDocumentMgr

Public Class UpdateFiles
    Shared ClasFact As ISwDMClassFactory = CreateObject("SwDocumentMgr.SwDMClassFactory.1")
    Shared dmDocMgr As SwDMApplication4 = ClasFact.GetApplication(sLicenseKey)

    Public Property FileName As IO.FileInfo

    Sub New(iFileName As String)
        FileName = New IO.FileInfo(iFileName)
    End Sub

    Function GetDocument(ByVal FullFileName As String) As SwDMDocument18
        If Not IO.File.Exists(FullFileName) Then Return Nothing
        Dim Errormsg As SwDmDocumentOpenError = SwDmDocumentOpenError.swDmDocumentOpenErrorNone 'swFileLoadError_e
        Dim Type As SwDmDocumentType
        Select Case IO.Path.GetExtension(FullFileName).ToLower
            Case ".sldprt"
                Type = SwDmDocumentType.swDmDocumentPart
            Case ".sldasm"
                Type = SwDmDocumentType.swDmDocumentAssembly
            Case ".slddraw"
                Type = SwDmDocumentType.swDmDocumentDrawing
        End Select

        Dim dmDoc As SwDMDocument18 = dmDocMgr.GetDocument(FullFileName, Type, True, Errormsg)
        If dmDoc IsNot Nothing AndAlso Errormsg = SwDmDocumentOpenError.swDmDocumentOpenErrorNone Then
            Return dmDoc
        Else
            Return Nothing
        End If
    End Function
    Friend Function Update(rules As ObservableCollection(Of ReplaceRules)) As Boolean

        If FileName.IsReadOnly Then Return False

        Dim dmDoc As SwDMDocument18 = GetDocument(FileName.FullName)
        If dmDoc Is Nothing Then Return False
        Try

            Dim dmExtRefOption As SwDMExternalReferenceOption2
            Dim swSearchOpt As SwDMSearchOption = dmDocMgr.GetSearchOptionObject
            dmExtRefOption = dmDocMgr.GetExternalReferenceOptionObject2
            dmExtRefOption.NeedSuppress = False
            dmExtRefOption.SearchOption = swSearchOpt

            Dim numExtRefs As Integer = dmDoc.GetExternalFeatureReferences2(dmExtRefOption)
            Dim extRefs() As String = dmExtRefOption.ExternalReferences
            Dim refConfigs() As String = dmExtRefOption.ReferencedConfigurations
            Dim brokenStatus() As Integer = dmExtRefOption.BrokenStatus
            Dim refIDs() As Integer = dmExtRefOption.IDs
            Dim ThisConfig As String = dmExtRefOption.Configuration

            Dim HasChanged As Boolean
            For index = 0 To numExtRefs - 1

                '  If brokenStatus(index) = SwDmReferenceStatus.swDmReferenceStatusNotBroken Then

                Dim RefFileName As String = extRefs(index).ToUpper
                Dim Child As New UpdateFiles(RefFileName)
                Child.Update(rules)
                For Each Rule As ReplaceRules In rules
                    If RefFileName.Contains(Rule.Find.ToUpper) Then
                        dmDoc.ReplaceReference(RefFileName, RefFileName.Replace(Rule.Find, Rule.ReplaceWith))
                        HasChanged = True
                    End If
                Next
            Next
            If HasChanged Then
                dmDoc.Save()
            End If
            Return True
        Finally
            dmDoc.CloseDoc()
        End Try
        Return False
    End Function
End Class
