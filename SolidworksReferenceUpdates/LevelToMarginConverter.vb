
Imports System.Globalization
<ValueConversion(GetType(Integer), GetType(Thickness))>
Public Class LevelToMarginConverter
    Implements IValueConverter

    Public Property LeftMargin() As Integer
        Get
            Return m_LeftMargin
        End Get
        Set
            m_LeftMargin = Value
        End Set
    End Property
    Private m_LeftMargin As Integer
    Public Property OtherMargin() As Integer
        Get
            Return m_OtherMargin
        End Get
        Set
            m_OtherMargin = Value
        End Set
    End Property
    Private m_OtherMargin As Integer

#Region "IValueConverter Members"

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim lvl As Integer = CInt(value)
        Return New Thickness(6 * lvl, 2, 2, 2)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
#End Region
End Class
