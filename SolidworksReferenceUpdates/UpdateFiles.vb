Imports System.Collections.ObjectModel
Imports System.Text.RegularExpressions
Imports SwDocumentMgr

Public Class UpdateFiles
    Shared dmDocMgr As SwDMApplication4 = GetSwDocMgr()

    Public Property FileName As IO.FileInfo

    Public Shared Results As New ObservableCollection(Of FileResults)
    Public Result As FileResults
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

        Dim dmDoc As SwDMDocument18 = dmDocMgr.GetDocument(FullFileName, Type, False, Errormsg)
        If dmDoc IsNot Nothing AndAlso Errormsg = SwDmDocumentOpenError.swDmDocumentOpenErrorNone Then
            Return dmDoc
        Else
            Return Nothing
        End If
    End Function
    Friend Sub CheckIflostReferences(dmDoc As SwDMDocument18)
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

            For index = 0 To numExtRefs - 1
                If brokenStatus(index) = SwDmReferenceStatus.swDmReferenceStatusBroken Then
                    Result.AddResult("Broken link", "Can not find " & extRefs(index), True)
                End If
            Next
        Catch
        End Try
    End Sub
    Friend Function Update(rules As ObservableCollection(Of ReplaceRules)) As Boolean

        If FileName.IsReadOnly Then Return False

        Dim dmDoc As SwDMDocument18 = GetDocument(FileName.FullName)
        If dmDoc Is Nothing Then Return False
        Try

            Result = New FileResults(FileName.FullName)
            Results.Add(Result)

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
                Try
                    Dim Refs() As String
                    Dim Status() As Integer
                    Dim swSearchOpt2 As ISwDMSearchOption = dmDocMgr.GetSearchOptionObject
                    Refs = dmDoc.GetAllExternalReferences4(swSearchOpt2, Status, Nothing, Nothing)
                Catch ex As Exception
                End Try

                Dim RefFileName As String = extRefs(index)
                For Each Rule As ReplaceRules In rules
                    If RefFileName.ToUpper.Contains(Rule.Find.ToUpper) Then
                        Dim NewReference As String = Regex.Replace(RefFileName, Rule.Find, Rule.ReplaceWith, RegexOptions.IgnoreCase)

                        dmDoc.ReplaceReference(RefFileName, NewReference)

                        HasChanged = True
                        Result.AddResult("Renamed", "Reference updated To " & NewReference, False)
                    End If
                Next

                Dim Child As New UpdateFiles(RefFileName)
                Child.Update(rules)
            Next


            If dmDoc.Save() <> SwDmDocumentSaveError.swDmDocumentSaveErrorNone Then
                    Result.AddResult("Error Savings", "", True)
                End If


            CheckIflostReferences(dmDoc)

            dmDoc.CloseDoc()
            dmDoc = Nothing

            Return True
        Catch
            Result.AddResult("Major Error", "Major crash", True)
        Finally
            If dmDoc IsNot Nothing Then
                dmDoc.CloseDoc()
                dmDoc = Nothing
            End If
        End Try
        Return False
    End Function
End Class
