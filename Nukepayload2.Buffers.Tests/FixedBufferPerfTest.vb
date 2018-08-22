Imports Nukepayload2.Buffers.Fixed

<TestClass>
Public Class FixedBufferPerfTest
    Const RepeatCount = 3000_0000

    <TestMethod>
    Sub TestArray()
        For i = 1 To RepeatCount
            Dim items(31) As Rect
            Dim endIndex = 0
            For j = 0 To 31
                items(j) = New Rect
                endIndex += 1
            Next
            For j = 0 To 31
                Dim item = items(j)
            Next
        Next
    End Sub

    <TestMethod>
    Sub TestListOfRect()
        For i = 1 To RepeatCount
            Dim items As New List(Of Rect)(32)
            For j = 1 To 32
                items.Add(New Rect)
            Next
            For j = 0 To 31
                Dim item = items(j)
            Next
        Next
    End Sub

    <TestMethod>
    Sub TestFixedList32OfRect()
        For i = 1 To RepeatCount
            Dim items As New FixedList32(Of Rect)
            For j = 1 To 32
                If Not items.TryAdd(New Rect) Then
                    Assert.Fail()
                End If
            Next
            For j = 0 To 31
                Dim item As Rect
                If Not items.TryGetItem(j, item) Then
                    Assert.Fail()
                End If
            Next
        Next
    End Sub

    <TestMethod>
    Sub TestFixedList32OfRectNoInlining()
        For i = 1 To RepeatCount
            Dim items As New FixedList32(Of Rect)
            For j = 1 To 32
                items.Add(New Rect)
            Next
            For j = 0 To 31
                Dim item = items(j)
            Next
        Next
    End Sub

    <TestMethod>
    Sub TestFixedList32OfRectUnsafe()
        For i = 1 To RepeatCount
            Dim items As New FixedList32(Of Rect)
            For j = 1 To 32
                items.UnsafeAdd(New Rect)
            Next
            For j = 0 To 31
                Dim item = items.UnsafeItem(j)
            Next
        Next
    End Sub
End Class
