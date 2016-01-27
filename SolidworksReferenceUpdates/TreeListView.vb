
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Controls.Primitives
Imports System.Globalization

''' <summary>
''' Represents a control that displays hierarchical data in a tree structure
''' that has items that can expand and collapse.
''' </summary>
Public Class TreeListView
    Inherits TreeView
    Shared Sub New()
        'Override the default style and the default control template
        DefaultStyleKeyProperty.OverrideMetadata(GetType(TreeListView), New FrameworkPropertyMetadata(GetType(TreeListView)))
    End Sub

    ''' <summary>
    ''' Initialize a new instance of TreeListView.
    ''' </summary>
    Public Sub New()
        Columns = New GridViewColumnCollection()
    End Sub

#Region "Properties"
    ''' <summary>
    ''' Gets or sets the collection of System.Windows.Controls.GridViewColumn 
    ''' objects that is defined for this TreeListView.
    ''' </summary>
    Public Property Columns() As GridViewColumnCollection
        Get
            Return DirectCast(GetValue(ColumnsProperty), GridViewColumnCollection)
        End Get
        Set
            SetValue(ColumnsProperty, Value)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether columns in a TreeListView can be
    ''' reordered by a drag-and-drop operation. This is a dependency property.
    ''' </summary>
    Public Property AllowsColumnReorder() As Boolean
        Get
            Return CBool(GetValue(AllowsColumnReorderProperty))
        End Get
        Set
            SetValue(AllowsColumnReorderProperty, Value)
        End Set
    End Property
#End Region

#Region "Static Dependency Properties"
    ' Using a DependencyProperty as the backing store for AllowsColumnReorder.  This enables animation, styling, binding, etc...
    Public Shared ReadOnly AllowsColumnReorderProperty As DependencyProperty = DependencyProperty.Register("AllowsColumnReorder", GetType(Boolean), GetType(TreeListView), New UIPropertyMetadata(Nothing))

    ' Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
    Public Shared ReadOnly ColumnsProperty As DependencyProperty = DependencyProperty.Register("Columns", GetType(GridViewColumnCollection), GetType(TreeListView), New UIPropertyMetadata(Nothing))
#End Region
End Class

''' <summary>
''' Represents a control that can switch states in order to expand a node of a TreeListView.
''' </summary>
Public Class TreeListViewExpander
    Inherits ToggleButton
End Class

''' <summary>
''' Represents a convert that can calculate the indentation of any element in a class derived from TreeView.
''' </summary>
<ValueConversion(GetType(Object), GetType(Double))>
Public Class TreeListViewConverter
    Implements IValueConverter
    Public Const Indentation As Double = 10

#Region "IValueConverter Members"

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        'If the value is null, don't return anything
        If value Is Nothing Then
            Return Nothing
        End If
        'Convert the item to a double
        If targetType = GetType(Double) AndAlso GetType(DependencyObject).IsAssignableFrom(value.[GetType]()) Then
            'Cast the item as a DependencyObject
            Dim Element As DependencyObject = TryCast(value, DependencyObject)
            'Create a level counter with value set to -1
            Dim Level As Integer = -1
            'Move up the visual tree and count the number of TreeViewItem's.
            While Element IsNot Nothing
                'Check whether the current elemeent is a TreeViewItem
                If GetType(TreeViewItem).IsAssignableFrom(Element.[GetType]()) Then
                    'Increase the level counter
                    Level += 1
                End If
                Element = VisualTreeHelper.GetParent(Element)
            End While
            'Return the indentation as a double
            Return Indentation * Level
        End If
        'Type conversion is not supported
        Throw New NotSupportedException(String.Format("Cannot convert from <{0}> to <{1}> using <TreeListViewConverter>.", value.[GetType](), targetType))
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Throw New NotSupportedException("This method is not supported.")
    End Function
#End Region
End Class



'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
