Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.CompilerServices

Namespace Fixed

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 2.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList2(Of T As Structure)
        Private _0 As T
        Private _1 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 2

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 2.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer2(Of T As Structure)
        Private _0 As T
        Private _1 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 2

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 4.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList4(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 4

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 4.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer4(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 4

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 8.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList8(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 8

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 8.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer8(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 8

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 16.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList16(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 16

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 16.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer16(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 16

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 32.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList32(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 32

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 32.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer32(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 32

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 64.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList64(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 64

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 64.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer64(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 64

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 128.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList128(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 128

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 128.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer128(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 128

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 256.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList256(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        Private _128 As T
        Private _129 As T
        Private _130 As T
        Private _131 As T
        Private _132 As T
        Private _133 As T
        Private _134 As T
        Private _135 As T
        Private _136 As T
        Private _137 As T
        Private _138 As T
        Private _139 As T
        Private _140 As T
        Private _141 As T
        Private _142 As T
        Private _143 As T
        Private _144 As T
        Private _145 As T
        Private _146 As T
        Private _147 As T
        Private _148 As T
        Private _149 As T
        Private _150 As T
        Private _151 As T
        Private _152 As T
        Private _153 As T
        Private _154 As T
        Private _155 As T
        Private _156 As T
        Private _157 As T
        Private _158 As T
        Private _159 As T
        Private _160 As T
        Private _161 As T
        Private _162 As T
        Private _163 As T
        Private _164 As T
        Private _165 As T
        Private _166 As T
        Private _167 As T
        Private _168 As T
        Private _169 As T
        Private _170 As T
        Private _171 As T
        Private _172 As T
        Private _173 As T
        Private _174 As T
        Private _175 As T
        Private _176 As T
        Private _177 As T
        Private _178 As T
        Private _179 As T
        Private _180 As T
        Private _181 As T
        Private _182 As T
        Private _183 As T
        Private _184 As T
        Private _185 As T
        Private _186 As T
        Private _187 As T
        Private _188 As T
        Private _189 As T
        Private _190 As T
        Private _191 As T
        Private _192 As T
        Private _193 As T
        Private _194 As T
        Private _195 As T
        Private _196 As T
        Private _197 As T
        Private _198 As T
        Private _199 As T
        Private _200 As T
        Private _201 As T
        Private _202 As T
        Private _203 As T
        Private _204 As T
        Private _205 As T
        Private _206 As T
        Private _207 As T
        Private _208 As T
        Private _209 As T
        Private _210 As T
        Private _211 As T
        Private _212 As T
        Private _213 As T
        Private _214 As T
        Private _215 As T
        Private _216 As T
        Private _217 As T
        Private _218 As T
        Private _219 As T
        Private _220 As T
        Private _221 As T
        Private _222 As T
        Private _223 As T
        Private _224 As T
        Private _225 As T
        Private _226 As T
        Private _227 As T
        Private _228 As T
        Private _229 As T
        Private _230 As T
        Private _231 As T
        Private _232 As T
        Private _233 As T
        Private _234 As T
        Private _235 As T
        Private _236 As T
        Private _237 As T
        Private _238 As T
        Private _239 As T
        Private _240 As T
        Private _241 As T
        Private _242 As T
        Private _243 As T
        Private _244 As T
        Private _245 As T
        Private _246 As T
        Private _247 As T
        Private _248 As T
        Private _249 As T
        Private _250 As T
        Private _251 As T
        Private _252 As T
        Private _253 As T
        Private _254 As T
        Private _255 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 256

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 256.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer256(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        Private _128 As T
        Private _129 As T
        Private _130 As T
        Private _131 As T
        Private _132 As T
        Private _133 As T
        Private _134 As T
        Private _135 As T
        Private _136 As T
        Private _137 As T
        Private _138 As T
        Private _139 As T
        Private _140 As T
        Private _141 As T
        Private _142 As T
        Private _143 As T
        Private _144 As T
        Private _145 As T
        Private _146 As T
        Private _147 As T
        Private _148 As T
        Private _149 As T
        Private _150 As T
        Private _151 As T
        Private _152 As T
        Private _153 As T
        Private _154 As T
        Private _155 As T
        Private _156 As T
        Private _157 As T
        Private _158 As T
        Private _159 As T
        Private _160 As T
        Private _161 As T
        Private _162 As T
        Private _163 As T
        Private _164 As T
        Private _165 As T
        Private _166 As T
        Private _167 As T
        Private _168 As T
        Private _169 As T
        Private _170 As T
        Private _171 As T
        Private _172 As T
        Private _173 As T
        Private _174 As T
        Private _175 As T
        Private _176 As T
        Private _177 As T
        Private _178 As T
        Private _179 As T
        Private _180 As T
        Private _181 As T
        Private _182 As T
        Private _183 As T
        Private _184 As T
        Private _185 As T
        Private _186 As T
        Private _187 As T
        Private _188 As T
        Private _189 As T
        Private _190 As T
        Private _191 As T
        Private _192 As T
        Private _193 As T
        Private _194 As T
        Private _195 As T
        Private _196 As T
        Private _197 As T
        Private _198 As T
        Private _199 As T
        Private _200 As T
        Private _201 As T
        Private _202 As T
        Private _203 As T
        Private _204 As T
        Private _205 As T
        Private _206 As T
        Private _207 As T
        Private _208 As T
        Private _209 As T
        Private _210 As T
        Private _211 As T
        Private _212 As T
        Private _213 As T
        Private _214 As T
        Private _215 As T
        Private _216 As T
        Private _217 As T
        Private _218 As T
        Private _219 As T
        Private _220 As T
        Private _221 As T
        Private _222 As T
        Private _223 As T
        Private _224 As T
        Private _225 As T
        Private _226 As T
        Private _227 As T
        Private _228 As T
        Private _229 As T
        Private _230 As T
        Private _231 As T
        Private _232 As T
        Private _233 As T
        Private _234 As T
        Private _235 As T
        Private _236 As T
        Private _237 As T
        Private _238 As T
        Private _239 As T
        Private _240 As T
        Private _241 As T
        Private _242 As T
        Private _243 As T
        Private _244 As T
        Private _245 As T
        Private _246 As T
        Private _247 As T
        Private _248 As T
        Private _249 As T
        Private _250 As T
        Private _251 As T
        Private _252 As T
        Private _253 As T
        Private _254 As T
        Private _255 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 256

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 512.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList512(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        Private _128 As T
        Private _129 As T
        Private _130 As T
        Private _131 As T
        Private _132 As T
        Private _133 As T
        Private _134 As T
        Private _135 As T
        Private _136 As T
        Private _137 As T
        Private _138 As T
        Private _139 As T
        Private _140 As T
        Private _141 As T
        Private _142 As T
        Private _143 As T
        Private _144 As T
        Private _145 As T
        Private _146 As T
        Private _147 As T
        Private _148 As T
        Private _149 As T
        Private _150 As T
        Private _151 As T
        Private _152 As T
        Private _153 As T
        Private _154 As T
        Private _155 As T
        Private _156 As T
        Private _157 As T
        Private _158 As T
        Private _159 As T
        Private _160 As T
        Private _161 As T
        Private _162 As T
        Private _163 As T
        Private _164 As T
        Private _165 As T
        Private _166 As T
        Private _167 As T
        Private _168 As T
        Private _169 As T
        Private _170 As T
        Private _171 As T
        Private _172 As T
        Private _173 As T
        Private _174 As T
        Private _175 As T
        Private _176 As T
        Private _177 As T
        Private _178 As T
        Private _179 As T
        Private _180 As T
        Private _181 As T
        Private _182 As T
        Private _183 As T
        Private _184 As T
        Private _185 As T
        Private _186 As T
        Private _187 As T
        Private _188 As T
        Private _189 As T
        Private _190 As T
        Private _191 As T
        Private _192 As T
        Private _193 As T
        Private _194 As T
        Private _195 As T
        Private _196 As T
        Private _197 As T
        Private _198 As T
        Private _199 As T
        Private _200 As T
        Private _201 As T
        Private _202 As T
        Private _203 As T
        Private _204 As T
        Private _205 As T
        Private _206 As T
        Private _207 As T
        Private _208 As T
        Private _209 As T
        Private _210 As T
        Private _211 As T
        Private _212 As T
        Private _213 As T
        Private _214 As T
        Private _215 As T
        Private _216 As T
        Private _217 As T
        Private _218 As T
        Private _219 As T
        Private _220 As T
        Private _221 As T
        Private _222 As T
        Private _223 As T
        Private _224 As T
        Private _225 As T
        Private _226 As T
        Private _227 As T
        Private _228 As T
        Private _229 As T
        Private _230 As T
        Private _231 As T
        Private _232 As T
        Private _233 As T
        Private _234 As T
        Private _235 As T
        Private _236 As T
        Private _237 As T
        Private _238 As T
        Private _239 As T
        Private _240 As T
        Private _241 As T
        Private _242 As T
        Private _243 As T
        Private _244 As T
        Private _245 As T
        Private _246 As T
        Private _247 As T
        Private _248 As T
        Private _249 As T
        Private _250 As T
        Private _251 As T
        Private _252 As T
        Private _253 As T
        Private _254 As T
        Private _255 As T
        Private _256 As T
        Private _257 As T
        Private _258 As T
        Private _259 As T
        Private _260 As T
        Private _261 As T
        Private _262 As T
        Private _263 As T
        Private _264 As T
        Private _265 As T
        Private _266 As T
        Private _267 As T
        Private _268 As T
        Private _269 As T
        Private _270 As T
        Private _271 As T
        Private _272 As T
        Private _273 As T
        Private _274 As T
        Private _275 As T
        Private _276 As T
        Private _277 As T
        Private _278 As T
        Private _279 As T
        Private _280 As T
        Private _281 As T
        Private _282 As T
        Private _283 As T
        Private _284 As T
        Private _285 As T
        Private _286 As T
        Private _287 As T
        Private _288 As T
        Private _289 As T
        Private _290 As T
        Private _291 As T
        Private _292 As T
        Private _293 As T
        Private _294 As T
        Private _295 As T
        Private _296 As T
        Private _297 As T
        Private _298 As T
        Private _299 As T
        Private _300 As T
        Private _301 As T
        Private _302 As T
        Private _303 As T
        Private _304 As T
        Private _305 As T
        Private _306 As T
        Private _307 As T
        Private _308 As T
        Private _309 As T
        Private _310 As T
        Private _311 As T
        Private _312 As T
        Private _313 As T
        Private _314 As T
        Private _315 As T
        Private _316 As T
        Private _317 As T
        Private _318 As T
        Private _319 As T
        Private _320 As T
        Private _321 As T
        Private _322 As T
        Private _323 As T
        Private _324 As T
        Private _325 As T
        Private _326 As T
        Private _327 As T
        Private _328 As T
        Private _329 As T
        Private _330 As T
        Private _331 As T
        Private _332 As T
        Private _333 As T
        Private _334 As T
        Private _335 As T
        Private _336 As T
        Private _337 As T
        Private _338 As T
        Private _339 As T
        Private _340 As T
        Private _341 As T
        Private _342 As T
        Private _343 As T
        Private _344 As T
        Private _345 As T
        Private _346 As T
        Private _347 As T
        Private _348 As T
        Private _349 As T
        Private _350 As T
        Private _351 As T
        Private _352 As T
        Private _353 As T
        Private _354 As T
        Private _355 As T
        Private _356 As T
        Private _357 As T
        Private _358 As T
        Private _359 As T
        Private _360 As T
        Private _361 As T
        Private _362 As T
        Private _363 As T
        Private _364 As T
        Private _365 As T
        Private _366 As T
        Private _367 As T
        Private _368 As T
        Private _369 As T
        Private _370 As T
        Private _371 As T
        Private _372 As T
        Private _373 As T
        Private _374 As T
        Private _375 As T
        Private _376 As T
        Private _377 As T
        Private _378 As T
        Private _379 As T
        Private _380 As T
        Private _381 As T
        Private _382 As T
        Private _383 As T
        Private _384 As T
        Private _385 As T
        Private _386 As T
        Private _387 As T
        Private _388 As T
        Private _389 As T
        Private _390 As T
        Private _391 As T
        Private _392 As T
        Private _393 As T
        Private _394 As T
        Private _395 As T
        Private _396 As T
        Private _397 As T
        Private _398 As T
        Private _399 As T
        Private _400 As T
        Private _401 As T
        Private _402 As T
        Private _403 As T
        Private _404 As T
        Private _405 As T
        Private _406 As T
        Private _407 As T
        Private _408 As T
        Private _409 As T
        Private _410 As T
        Private _411 As T
        Private _412 As T
        Private _413 As T
        Private _414 As T
        Private _415 As T
        Private _416 As T
        Private _417 As T
        Private _418 As T
        Private _419 As T
        Private _420 As T
        Private _421 As T
        Private _422 As T
        Private _423 As T
        Private _424 As T
        Private _425 As T
        Private _426 As T
        Private _427 As T
        Private _428 As T
        Private _429 As T
        Private _430 As T
        Private _431 As T
        Private _432 As T
        Private _433 As T
        Private _434 As T
        Private _435 As T
        Private _436 As T
        Private _437 As T
        Private _438 As T
        Private _439 As T
        Private _440 As T
        Private _441 As T
        Private _442 As T
        Private _443 As T
        Private _444 As T
        Private _445 As T
        Private _446 As T
        Private _447 As T
        Private _448 As T
        Private _449 As T
        Private _450 As T
        Private _451 As T
        Private _452 As T
        Private _453 As T
        Private _454 As T
        Private _455 As T
        Private _456 As T
        Private _457 As T
        Private _458 As T
        Private _459 As T
        Private _460 As T
        Private _461 As T
        Private _462 As T
        Private _463 As T
        Private _464 As T
        Private _465 As T
        Private _466 As T
        Private _467 As T
        Private _468 As T
        Private _469 As T
        Private _470 As T
        Private _471 As T
        Private _472 As T
        Private _473 As T
        Private _474 As T
        Private _475 As T
        Private _476 As T
        Private _477 As T
        Private _478 As T
        Private _479 As T
        Private _480 As T
        Private _481 As T
        Private _482 As T
        Private _483 As T
        Private _484 As T
        Private _485 As T
        Private _486 As T
        Private _487 As T
        Private _488 As T
        Private _489 As T
        Private _490 As T
        Private _491 As T
        Private _492 As T
        Private _493 As T
        Private _494 As T
        Private _495 As T
        Private _496 As T
        Private _497 As T
        Private _498 As T
        Private _499 As T
        Private _500 As T
        Private _501 As T
        Private _502 As T
        Private _503 As T
        Private _504 As T
        Private _505 As T
        Private _506 As T
        Private _507 As T
        Private _508 As T
        Private _509 As T
        Private _510 As T
        Private _511 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 512

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 512.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer512(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        Private _128 As T
        Private _129 As T
        Private _130 As T
        Private _131 As T
        Private _132 As T
        Private _133 As T
        Private _134 As T
        Private _135 As T
        Private _136 As T
        Private _137 As T
        Private _138 As T
        Private _139 As T
        Private _140 As T
        Private _141 As T
        Private _142 As T
        Private _143 As T
        Private _144 As T
        Private _145 As T
        Private _146 As T
        Private _147 As T
        Private _148 As T
        Private _149 As T
        Private _150 As T
        Private _151 As T
        Private _152 As T
        Private _153 As T
        Private _154 As T
        Private _155 As T
        Private _156 As T
        Private _157 As T
        Private _158 As T
        Private _159 As T
        Private _160 As T
        Private _161 As T
        Private _162 As T
        Private _163 As T
        Private _164 As T
        Private _165 As T
        Private _166 As T
        Private _167 As T
        Private _168 As T
        Private _169 As T
        Private _170 As T
        Private _171 As T
        Private _172 As T
        Private _173 As T
        Private _174 As T
        Private _175 As T
        Private _176 As T
        Private _177 As T
        Private _178 As T
        Private _179 As T
        Private _180 As T
        Private _181 As T
        Private _182 As T
        Private _183 As T
        Private _184 As T
        Private _185 As T
        Private _186 As T
        Private _187 As T
        Private _188 As T
        Private _189 As T
        Private _190 As T
        Private _191 As T
        Private _192 As T
        Private _193 As T
        Private _194 As T
        Private _195 As T
        Private _196 As T
        Private _197 As T
        Private _198 As T
        Private _199 As T
        Private _200 As T
        Private _201 As T
        Private _202 As T
        Private _203 As T
        Private _204 As T
        Private _205 As T
        Private _206 As T
        Private _207 As T
        Private _208 As T
        Private _209 As T
        Private _210 As T
        Private _211 As T
        Private _212 As T
        Private _213 As T
        Private _214 As T
        Private _215 As T
        Private _216 As T
        Private _217 As T
        Private _218 As T
        Private _219 As T
        Private _220 As T
        Private _221 As T
        Private _222 As T
        Private _223 As T
        Private _224 As T
        Private _225 As T
        Private _226 As T
        Private _227 As T
        Private _228 As T
        Private _229 As T
        Private _230 As T
        Private _231 As T
        Private _232 As T
        Private _233 As T
        Private _234 As T
        Private _235 As T
        Private _236 As T
        Private _237 As T
        Private _238 As T
        Private _239 As T
        Private _240 As T
        Private _241 As T
        Private _242 As T
        Private _243 As T
        Private _244 As T
        Private _245 As T
        Private _246 As T
        Private _247 As T
        Private _248 As T
        Private _249 As T
        Private _250 As T
        Private _251 As T
        Private _252 As T
        Private _253 As T
        Private _254 As T
        Private _255 As T
        Private _256 As T
        Private _257 As T
        Private _258 As T
        Private _259 As T
        Private _260 As T
        Private _261 As T
        Private _262 As T
        Private _263 As T
        Private _264 As T
        Private _265 As T
        Private _266 As T
        Private _267 As T
        Private _268 As T
        Private _269 As T
        Private _270 As T
        Private _271 As T
        Private _272 As T
        Private _273 As T
        Private _274 As T
        Private _275 As T
        Private _276 As T
        Private _277 As T
        Private _278 As T
        Private _279 As T
        Private _280 As T
        Private _281 As T
        Private _282 As T
        Private _283 As T
        Private _284 As T
        Private _285 As T
        Private _286 As T
        Private _287 As T
        Private _288 As T
        Private _289 As T
        Private _290 As T
        Private _291 As T
        Private _292 As T
        Private _293 As T
        Private _294 As T
        Private _295 As T
        Private _296 As T
        Private _297 As T
        Private _298 As T
        Private _299 As T
        Private _300 As T
        Private _301 As T
        Private _302 As T
        Private _303 As T
        Private _304 As T
        Private _305 As T
        Private _306 As T
        Private _307 As T
        Private _308 As T
        Private _309 As T
        Private _310 As T
        Private _311 As T
        Private _312 As T
        Private _313 As T
        Private _314 As T
        Private _315 As T
        Private _316 As T
        Private _317 As T
        Private _318 As T
        Private _319 As T
        Private _320 As T
        Private _321 As T
        Private _322 As T
        Private _323 As T
        Private _324 As T
        Private _325 As T
        Private _326 As T
        Private _327 As T
        Private _328 As T
        Private _329 As T
        Private _330 As T
        Private _331 As T
        Private _332 As T
        Private _333 As T
        Private _334 As T
        Private _335 As T
        Private _336 As T
        Private _337 As T
        Private _338 As T
        Private _339 As T
        Private _340 As T
        Private _341 As T
        Private _342 As T
        Private _343 As T
        Private _344 As T
        Private _345 As T
        Private _346 As T
        Private _347 As T
        Private _348 As T
        Private _349 As T
        Private _350 As T
        Private _351 As T
        Private _352 As T
        Private _353 As T
        Private _354 As T
        Private _355 As T
        Private _356 As T
        Private _357 As T
        Private _358 As T
        Private _359 As T
        Private _360 As T
        Private _361 As T
        Private _362 As T
        Private _363 As T
        Private _364 As T
        Private _365 As T
        Private _366 As T
        Private _367 As T
        Private _368 As T
        Private _369 As T
        Private _370 As T
        Private _371 As T
        Private _372 As T
        Private _373 As T
        Private _374 As T
        Private _375 As T
        Private _376 As T
        Private _377 As T
        Private _378 As T
        Private _379 As T
        Private _380 As T
        Private _381 As T
        Private _382 As T
        Private _383 As T
        Private _384 As T
        Private _385 As T
        Private _386 As T
        Private _387 As T
        Private _388 As T
        Private _389 As T
        Private _390 As T
        Private _391 As T
        Private _392 As T
        Private _393 As T
        Private _394 As T
        Private _395 As T
        Private _396 As T
        Private _397 As T
        Private _398 As T
        Private _399 As T
        Private _400 As T
        Private _401 As T
        Private _402 As T
        Private _403 As T
        Private _404 As T
        Private _405 As T
        Private _406 As T
        Private _407 As T
        Private _408 As T
        Private _409 As T
        Private _410 As T
        Private _411 As T
        Private _412 As T
        Private _413 As T
        Private _414 As T
        Private _415 As T
        Private _416 As T
        Private _417 As T
        Private _418 As T
        Private _419 As T
        Private _420 As T
        Private _421 As T
        Private _422 As T
        Private _423 As T
        Private _424 As T
        Private _425 As T
        Private _426 As T
        Private _427 As T
        Private _428 As T
        Private _429 As T
        Private _430 As T
        Private _431 As T
        Private _432 As T
        Private _433 As T
        Private _434 As T
        Private _435 As T
        Private _436 As T
        Private _437 As T
        Private _438 As T
        Private _439 As T
        Private _440 As T
        Private _441 As T
        Private _442 As T
        Private _443 As T
        Private _444 As T
        Private _445 As T
        Private _446 As T
        Private _447 As T
        Private _448 As T
        Private _449 As T
        Private _450 As T
        Private _451 As T
        Private _452 As T
        Private _453 As T
        Private _454 As T
        Private _455 As T
        Private _456 As T
        Private _457 As T
        Private _458 As T
        Private _459 As T
        Private _460 As T
        Private _461 As T
        Private _462 As T
        Private _463 As T
        Private _464 As T
        Private _465 As T
        Private _466 As T
        Private _467 As T
        Private _468 As T
        Private _469 As T
        Private _470 As T
        Private _471 As T
        Private _472 As T
        Private _473 As T
        Private _474 As T
        Private _475 As T
        Private _476 As T
        Private _477 As T
        Private _478 As T
        Private _479 As T
        Private _480 As T
        Private _481 As T
        Private _482 As T
        Private _483 As T
        Private _484 As T
        Private _485 As T
        Private _486 As T
        Private _487 As T
        Private _488 As T
        Private _489 As T
        Private _490 As T
        Private _491 As T
        Private _492 As T
        Private _493 As T
        Private _494 As T
        Private _495 As T
        Private _496 As T
        Private _497 As T
        Private _498 As T
        Private _499 As T
        Private _500 As T
        Private _501 As T
        Private _502 As T
        Private _503 As T
        Private _504 As T
        Private _505 As T
        Private _506 As T
        Private _507 As T
        Private _508 As T
        Private _509 As T
        Private _510 As T
        Private _511 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 512

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated list of <typeparamref name="T"/> * 1024.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedList1024(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        Private _128 As T
        Private _129 As T
        Private _130 As T
        Private _131 As T
        Private _132 As T
        Private _133 As T
        Private _134 As T
        Private _135 As T
        Private _136 As T
        Private _137 As T
        Private _138 As T
        Private _139 As T
        Private _140 As T
        Private _141 As T
        Private _142 As T
        Private _143 As T
        Private _144 As T
        Private _145 As T
        Private _146 As T
        Private _147 As T
        Private _148 As T
        Private _149 As T
        Private _150 As T
        Private _151 As T
        Private _152 As T
        Private _153 As T
        Private _154 As T
        Private _155 As T
        Private _156 As T
        Private _157 As T
        Private _158 As T
        Private _159 As T
        Private _160 As T
        Private _161 As T
        Private _162 As T
        Private _163 As T
        Private _164 As T
        Private _165 As T
        Private _166 As T
        Private _167 As T
        Private _168 As T
        Private _169 As T
        Private _170 As T
        Private _171 As T
        Private _172 As T
        Private _173 As T
        Private _174 As T
        Private _175 As T
        Private _176 As T
        Private _177 As T
        Private _178 As T
        Private _179 As T
        Private _180 As T
        Private _181 As T
        Private _182 As T
        Private _183 As T
        Private _184 As T
        Private _185 As T
        Private _186 As T
        Private _187 As T
        Private _188 As T
        Private _189 As T
        Private _190 As T
        Private _191 As T
        Private _192 As T
        Private _193 As T
        Private _194 As T
        Private _195 As T
        Private _196 As T
        Private _197 As T
        Private _198 As T
        Private _199 As T
        Private _200 As T
        Private _201 As T
        Private _202 As T
        Private _203 As T
        Private _204 As T
        Private _205 As T
        Private _206 As T
        Private _207 As T
        Private _208 As T
        Private _209 As T
        Private _210 As T
        Private _211 As T
        Private _212 As T
        Private _213 As T
        Private _214 As T
        Private _215 As T
        Private _216 As T
        Private _217 As T
        Private _218 As T
        Private _219 As T
        Private _220 As T
        Private _221 As T
        Private _222 As T
        Private _223 As T
        Private _224 As T
        Private _225 As T
        Private _226 As T
        Private _227 As T
        Private _228 As T
        Private _229 As T
        Private _230 As T
        Private _231 As T
        Private _232 As T
        Private _233 As T
        Private _234 As T
        Private _235 As T
        Private _236 As T
        Private _237 As T
        Private _238 As T
        Private _239 As T
        Private _240 As T
        Private _241 As T
        Private _242 As T
        Private _243 As T
        Private _244 As T
        Private _245 As T
        Private _246 As T
        Private _247 As T
        Private _248 As T
        Private _249 As T
        Private _250 As T
        Private _251 As T
        Private _252 As T
        Private _253 As T
        Private _254 As T
        Private _255 As T
        Private _256 As T
        Private _257 As T
        Private _258 As T
        Private _259 As T
        Private _260 As T
        Private _261 As T
        Private _262 As T
        Private _263 As T
        Private _264 As T
        Private _265 As T
        Private _266 As T
        Private _267 As T
        Private _268 As T
        Private _269 As T
        Private _270 As T
        Private _271 As T
        Private _272 As T
        Private _273 As T
        Private _274 As T
        Private _275 As T
        Private _276 As T
        Private _277 As T
        Private _278 As T
        Private _279 As T
        Private _280 As T
        Private _281 As T
        Private _282 As T
        Private _283 As T
        Private _284 As T
        Private _285 As T
        Private _286 As T
        Private _287 As T
        Private _288 As T
        Private _289 As T
        Private _290 As T
        Private _291 As T
        Private _292 As T
        Private _293 As T
        Private _294 As T
        Private _295 As T
        Private _296 As T
        Private _297 As T
        Private _298 As T
        Private _299 As T
        Private _300 As T
        Private _301 As T
        Private _302 As T
        Private _303 As T
        Private _304 As T
        Private _305 As T
        Private _306 As T
        Private _307 As T
        Private _308 As T
        Private _309 As T
        Private _310 As T
        Private _311 As T
        Private _312 As T
        Private _313 As T
        Private _314 As T
        Private _315 As T
        Private _316 As T
        Private _317 As T
        Private _318 As T
        Private _319 As T
        Private _320 As T
        Private _321 As T
        Private _322 As T
        Private _323 As T
        Private _324 As T
        Private _325 As T
        Private _326 As T
        Private _327 As T
        Private _328 As T
        Private _329 As T
        Private _330 As T
        Private _331 As T
        Private _332 As T
        Private _333 As T
        Private _334 As T
        Private _335 As T
        Private _336 As T
        Private _337 As T
        Private _338 As T
        Private _339 As T
        Private _340 As T
        Private _341 As T
        Private _342 As T
        Private _343 As T
        Private _344 As T
        Private _345 As T
        Private _346 As T
        Private _347 As T
        Private _348 As T
        Private _349 As T
        Private _350 As T
        Private _351 As T
        Private _352 As T
        Private _353 As T
        Private _354 As T
        Private _355 As T
        Private _356 As T
        Private _357 As T
        Private _358 As T
        Private _359 As T
        Private _360 As T
        Private _361 As T
        Private _362 As T
        Private _363 As T
        Private _364 As T
        Private _365 As T
        Private _366 As T
        Private _367 As T
        Private _368 As T
        Private _369 As T
        Private _370 As T
        Private _371 As T
        Private _372 As T
        Private _373 As T
        Private _374 As T
        Private _375 As T
        Private _376 As T
        Private _377 As T
        Private _378 As T
        Private _379 As T
        Private _380 As T
        Private _381 As T
        Private _382 As T
        Private _383 As T
        Private _384 As T
        Private _385 As T
        Private _386 As T
        Private _387 As T
        Private _388 As T
        Private _389 As T
        Private _390 As T
        Private _391 As T
        Private _392 As T
        Private _393 As T
        Private _394 As T
        Private _395 As T
        Private _396 As T
        Private _397 As T
        Private _398 As T
        Private _399 As T
        Private _400 As T
        Private _401 As T
        Private _402 As T
        Private _403 As T
        Private _404 As T
        Private _405 As T
        Private _406 As T
        Private _407 As T
        Private _408 As T
        Private _409 As T
        Private _410 As T
        Private _411 As T
        Private _412 As T
        Private _413 As T
        Private _414 As T
        Private _415 As T
        Private _416 As T
        Private _417 As T
        Private _418 As T
        Private _419 As T
        Private _420 As T
        Private _421 As T
        Private _422 As T
        Private _423 As T
        Private _424 As T
        Private _425 As T
        Private _426 As T
        Private _427 As T
        Private _428 As T
        Private _429 As T
        Private _430 As T
        Private _431 As T
        Private _432 As T
        Private _433 As T
        Private _434 As T
        Private _435 As T
        Private _436 As T
        Private _437 As T
        Private _438 As T
        Private _439 As T
        Private _440 As T
        Private _441 As T
        Private _442 As T
        Private _443 As T
        Private _444 As T
        Private _445 As T
        Private _446 As T
        Private _447 As T
        Private _448 As T
        Private _449 As T
        Private _450 As T
        Private _451 As T
        Private _452 As T
        Private _453 As T
        Private _454 As T
        Private _455 As T
        Private _456 As T
        Private _457 As T
        Private _458 As T
        Private _459 As T
        Private _460 As T
        Private _461 As T
        Private _462 As T
        Private _463 As T
        Private _464 As T
        Private _465 As T
        Private _466 As T
        Private _467 As T
        Private _468 As T
        Private _469 As T
        Private _470 As T
        Private _471 As T
        Private _472 As T
        Private _473 As T
        Private _474 As T
        Private _475 As T
        Private _476 As T
        Private _477 As T
        Private _478 As T
        Private _479 As T
        Private _480 As T
        Private _481 As T
        Private _482 As T
        Private _483 As T
        Private _484 As T
        Private _485 As T
        Private _486 As T
        Private _487 As T
        Private _488 As T
        Private _489 As T
        Private _490 As T
        Private _491 As T
        Private _492 As T
        Private _493 As T
        Private _494 As T
        Private _495 As T
        Private _496 As T
        Private _497 As T
        Private _498 As T
        Private _499 As T
        Private _500 As T
        Private _501 As T
        Private _502 As T
        Private _503 As T
        Private _504 As T
        Private _505 As T
        Private _506 As T
        Private _507 As T
        Private _508 As T
        Private _509 As T
        Private _510 As T
        Private _511 As T
        Private _512 As T
        Private _513 As T
        Private _514 As T
        Private _515 As T
        Private _516 As T
        Private _517 As T
        Private _518 As T
        Private _519 As T
        Private _520 As T
        Private _521 As T
        Private _522 As T
        Private _523 As T
        Private _524 As T
        Private _525 As T
        Private _526 As T
        Private _527 As T
        Private _528 As T
        Private _529 As T
        Private _530 As T
        Private _531 As T
        Private _532 As T
        Private _533 As T
        Private _534 As T
        Private _535 As T
        Private _536 As T
        Private _537 As T
        Private _538 As T
        Private _539 As T
        Private _540 As T
        Private _541 As T
        Private _542 As T
        Private _543 As T
        Private _544 As T
        Private _545 As T
        Private _546 As T
        Private _547 As T
        Private _548 As T
        Private _549 As T
        Private _550 As T
        Private _551 As T
        Private _552 As T
        Private _553 As T
        Private _554 As T
        Private _555 As T
        Private _556 As T
        Private _557 As T
        Private _558 As T
        Private _559 As T
        Private _560 As T
        Private _561 As T
        Private _562 As T
        Private _563 As T
        Private _564 As T
        Private _565 As T
        Private _566 As T
        Private _567 As T
        Private _568 As T
        Private _569 As T
        Private _570 As T
        Private _571 As T
        Private _572 As T
        Private _573 As T
        Private _574 As T
        Private _575 As T
        Private _576 As T
        Private _577 As T
        Private _578 As T
        Private _579 As T
        Private _580 As T
        Private _581 As T
        Private _582 As T
        Private _583 As T
        Private _584 As T
        Private _585 As T
        Private _586 As T
        Private _587 As T
        Private _588 As T
        Private _589 As T
        Private _590 As T
        Private _591 As T
        Private _592 As T
        Private _593 As T
        Private _594 As T
        Private _595 As T
        Private _596 As T
        Private _597 As T
        Private _598 As T
        Private _599 As T
        Private _600 As T
        Private _601 As T
        Private _602 As T
        Private _603 As T
        Private _604 As T
        Private _605 As T
        Private _606 As T
        Private _607 As T
        Private _608 As T
        Private _609 As T
        Private _610 As T
        Private _611 As T
        Private _612 As T
        Private _613 As T
        Private _614 As T
        Private _615 As T
        Private _616 As T
        Private _617 As T
        Private _618 As T
        Private _619 As T
        Private _620 As T
        Private _621 As T
        Private _622 As T
        Private _623 As T
        Private _624 As T
        Private _625 As T
        Private _626 As T
        Private _627 As T
        Private _628 As T
        Private _629 As T
        Private _630 As T
        Private _631 As T
        Private _632 As T
        Private _633 As T
        Private _634 As T
        Private _635 As T
        Private _636 As T
        Private _637 As T
        Private _638 As T
        Private _639 As T
        Private _640 As T
        Private _641 As T
        Private _642 As T
        Private _643 As T
        Private _644 As T
        Private _645 As T
        Private _646 As T
        Private _647 As T
        Private _648 As T
        Private _649 As T
        Private _650 As T
        Private _651 As T
        Private _652 As T
        Private _653 As T
        Private _654 As T
        Private _655 As T
        Private _656 As T
        Private _657 As T
        Private _658 As T
        Private _659 As T
        Private _660 As T
        Private _661 As T
        Private _662 As T
        Private _663 As T
        Private _664 As T
        Private _665 As T
        Private _666 As T
        Private _667 As T
        Private _668 As T
        Private _669 As T
        Private _670 As T
        Private _671 As T
        Private _672 As T
        Private _673 As T
        Private _674 As T
        Private _675 As T
        Private _676 As T
        Private _677 As T
        Private _678 As T
        Private _679 As T
        Private _680 As T
        Private _681 As T
        Private _682 As T
        Private _683 As T
        Private _684 As T
        Private _685 As T
        Private _686 As T
        Private _687 As T
        Private _688 As T
        Private _689 As T
        Private _690 As T
        Private _691 As T
        Private _692 As T
        Private _693 As T
        Private _694 As T
        Private _695 As T
        Private _696 As T
        Private _697 As T
        Private _698 As T
        Private _699 As T
        Private _700 As T
        Private _701 As T
        Private _702 As T
        Private _703 As T
        Private _704 As T
        Private _705 As T
        Private _706 As T
        Private _707 As T
        Private _708 As T
        Private _709 As T
        Private _710 As T
        Private _711 As T
        Private _712 As T
        Private _713 As T
        Private _714 As T
        Private _715 As T
        Private _716 As T
        Private _717 As T
        Private _718 As T
        Private _719 As T
        Private _720 As T
        Private _721 As T
        Private _722 As T
        Private _723 As T
        Private _724 As T
        Private _725 As T
        Private _726 As T
        Private _727 As T
        Private _728 As T
        Private _729 As T
        Private _730 As T
        Private _731 As T
        Private _732 As T
        Private _733 As T
        Private _734 As T
        Private _735 As T
        Private _736 As T
        Private _737 As T
        Private _738 As T
        Private _739 As T
        Private _740 As T
        Private _741 As T
        Private _742 As T
        Private _743 As T
        Private _744 As T
        Private _745 As T
        Private _746 As T
        Private _747 As T
        Private _748 As T
        Private _749 As T
        Private _750 As T
        Private _751 As T
        Private _752 As T
        Private _753 As T
        Private _754 As T
        Private _755 As T
        Private _756 As T
        Private _757 As T
        Private _758 As T
        Private _759 As T
        Private _760 As T
        Private _761 As T
        Private _762 As T
        Private _763 As T
        Private _764 As T
        Private _765 As T
        Private _766 As T
        Private _767 As T
        Private _768 As T
        Private _769 As T
        Private _770 As T
        Private _771 As T
        Private _772 As T
        Private _773 As T
        Private _774 As T
        Private _775 As T
        Private _776 As T
        Private _777 As T
        Private _778 As T
        Private _779 As T
        Private _780 As T
        Private _781 As T
        Private _782 As T
        Private _783 As T
        Private _784 As T
        Private _785 As T
        Private _786 As T
        Private _787 As T
        Private _788 As T
        Private _789 As T
        Private _790 As T
        Private _791 As T
        Private _792 As T
        Private _793 As T
        Private _794 As T
        Private _795 As T
        Private _796 As T
        Private _797 As T
        Private _798 As T
        Private _799 As T
        Private _800 As T
        Private _801 As T
        Private _802 As T
        Private _803 As T
        Private _804 As T
        Private _805 As T
        Private _806 As T
        Private _807 As T
        Private _808 As T
        Private _809 As T
        Private _810 As T
        Private _811 As T
        Private _812 As T
        Private _813 As T
        Private _814 As T
        Private _815 As T
        Private _816 As T
        Private _817 As T
        Private _818 As T
        Private _819 As T
        Private _820 As T
        Private _821 As T
        Private _822 As T
        Private _823 As T
        Private _824 As T
        Private _825 As T
        Private _826 As T
        Private _827 As T
        Private _828 As T
        Private _829 As T
        Private _830 As T
        Private _831 As T
        Private _832 As T
        Private _833 As T
        Private _834 As T
        Private _835 As T
        Private _836 As T
        Private _837 As T
        Private _838 As T
        Private _839 As T
        Private _840 As T
        Private _841 As T
        Private _842 As T
        Private _843 As T
        Private _844 As T
        Private _845 As T
        Private _846 As T
        Private _847 As T
        Private _848 As T
        Private _849 As T
        Private _850 As T
        Private _851 As T
        Private _852 As T
        Private _853 As T
        Private _854 As T
        Private _855 As T
        Private _856 As T
        Private _857 As T
        Private _858 As T
        Private _859 As T
        Private _860 As T
        Private _861 As T
        Private _862 As T
        Private _863 As T
        Private _864 As T
        Private _865 As T
        Private _866 As T
        Private _867 As T
        Private _868 As T
        Private _869 As T
        Private _870 As T
        Private _871 As T
        Private _872 As T
        Private _873 As T
        Private _874 As T
        Private _875 As T
        Private _876 As T
        Private _877 As T
        Private _878 As T
        Private _879 As T
        Private _880 As T
        Private _881 As T
        Private _882 As T
        Private _883 As T
        Private _884 As T
        Private _885 As T
        Private _886 As T
        Private _887 As T
        Private _888 As T
        Private _889 As T
        Private _890 As T
        Private _891 As T
        Private _892 As T
        Private _893 As T
        Private _894 As T
        Private _895 As T
        Private _896 As T
        Private _897 As T
        Private _898 As T
        Private _899 As T
        Private _900 As T
        Private _901 As T
        Private _902 As T
        Private _903 As T
        Private _904 As T
        Private _905 As T
        Private _906 As T
        Private _907 As T
        Private _908 As T
        Private _909 As T
        Private _910 As T
        Private _911 As T
        Private _912 As T
        Private _913 As T
        Private _914 As T
        Private _915 As T
        Private _916 As T
        Private _917 As T
        Private _918 As T
        Private _919 As T
        Private _920 As T
        Private _921 As T
        Private _922 As T
        Private _923 As T
        Private _924 As T
        Private _925 As T
        Private _926 As T
        Private _927 As T
        Private _928 As T
        Private _929 As T
        Private _930 As T
        Private _931 As T
        Private _932 As T
        Private _933 As T
        Private _934 As T
        Private _935 As T
        Private _936 As T
        Private _937 As T
        Private _938 As T
        Private _939 As T
        Private _940 As T
        Private _941 As T
        Private _942 As T
        Private _943 As T
        Private _944 As T
        Private _945 As T
        Private _946 As T
        Private _947 As T
        Private _948 As T
        Private _949 As T
        Private _950 As T
        Private _951 As T
        Private _952 As T
        Private _953 As T
        Private _954 As T
        Private _955 As T
        Private _956 As T
        Private _957 As T
        Private _958 As T
        Private _959 As T
        Private _960 As T
        Private _961 As T
        Private _962 As T
        Private _963 As T
        Private _964 As T
        Private _965 As T
        Private _966 As T
        Private _967 As T
        Private _968 As T
        Private _969 As T
        Private _970 As T
        Private _971 As T
        Private _972 As T
        Private _973 As T
        Private _974 As T
        Private _975 As T
        Private _976 As T
        Private _977 As T
        Private _978 As T
        Private _979 As T
        Private _980 As T
        Private _981 As T
        Private _982 As T
        Private _983 As T
        Private _984 As T
        Private _985 As T
        Private _986 As T
        Private _987 As T
        Private _988 As T
        Private _989 As T
        Private _990 As T
        Private _991 As T
        Private _992 As T
        Private _993 As T
        Private _994 As T
        Private _995 As T
        Private _996 As T
        Private _997 As T
        Private _998 As T
        Private _999 As T
        Private _1000 As T
        Private _1001 As T
        Private _1002 As T
        Private _1003 As T
        Private _1004 As T
        Private _1005 As T
        Private _1006 As T
        Private _1007 As T
        Private _1008 As T
        Private _1009 As T
        Private _1010 As T
        Private _1011 As T
        Private _1012 As T
        Private _1013 As T
        Private _1014 As T
        Private _1015 As T
        Private _1016 As T
        Private _1017 As T
        Private _1018 As T
        Private _1019 As T
        Private _1020 As T
        Private _1021 As T
        Private _1022 As T
        Private _1023 As T
        ''' <summary>
        ''' The length of the current buffer. It is between 0 (inclusive) and <see cref="MaxCount"/> (exclusive).
        ''' </summary>
        Public Count As Integer
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 1024

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= Count Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= Count Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list. Returns whether the element was added.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <returns>If returns <see langword="True"/>, the item was added successfully. Otherwise, the item can't be added.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryAdd(value As T) As Boolean
            If Count >= MaxCount Then Return False
            Unsafe.Add(_0, Count) = value
            Count += 1
            Return True
        End Function

        ''' <summary>
        ''' Adds a new value to the list.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        ''' <exception cref="OverflowException"/>
        Public Sub Add(value As T)
            If Count >= MaxCount Then Throw New OverflowException
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Adds a new value to the list without boundary check.
        ''' </summary>
        ''' <param name="value">The new value to be added.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeAdd(value As T)
            Unsafe.Add(_0, Count) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub RemoveAt(index As Integer)
            If index >= Count OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub
        
        ''' <summary>
        ''' Removes an item from the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeRemoveAt(index As Integer)
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
        End Sub

        ''' <summary>
        ''' Removes an item from the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <returns>Whether the operation succeed.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryRemoveAt(index As Integer) As Boolean
            If index >= Count OrElse index < 0 Then Return False
            For i = index To Count - 2
                Unsafe.Add(_0, i) = Unsafe.Add(_0, i + 1)
            Next
            Count -= 1
            Return True
        End Function

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        ''' <exception cref="IndexOutOfRangeException"/>
        Public Sub Insert(index As Integer, value As T)
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Throw New IndexOutOfRangeException
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub
        
        ''' <summary>
        ''' Inserts an item into the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the item.</param>
        ''' <param name="value">The new value.</param>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub UnsafeInsert(index As Integer, value As T)
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
        End Sub

        ''' <summary>
        ''' Inserts an item into the specified index.
        ''' </summary>
        ''' <param name="index">The index of the new item.</param>
        ''' <param name="value">The new value.</param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryInsert(index As Integer, value As T) As Boolean
            If index > Count OrElse index >= MaxCount OrElse index < 0 Then Return False
            For i = Count - 1 To index Step -1
                Unsafe.Add(_0, i + 1) = Unsafe.Add(_0, i)
            Next
            Unsafe.Add(_0, index) = value
            Count += 1
            Return True
        End Function
    End Structure

    ''' <summary>
    ''' Stack allocated <typeparamref name="T"/> * 1024.
    ''' </summary>
    <SuppressMessage("", "IDE0044")>
    <UnsafeValueType>
    Public Structure FixedBuffer1024(Of T As Structure)
        Private _0 As T
        Private _1 As T
        Private _2 As T
        Private _3 As T
        Private _4 As T
        Private _5 As T
        Private _6 As T
        Private _7 As T
        Private _8 As T
        Private _9 As T
        Private _10 As T
        Private _11 As T
        Private _12 As T
        Private _13 As T
        Private _14 As T
        Private _15 As T
        Private _16 As T
        Private _17 As T
        Private _18 As T
        Private _19 As T
        Private _20 As T
        Private _21 As T
        Private _22 As T
        Private _23 As T
        Private _24 As T
        Private _25 As T
        Private _26 As T
        Private _27 As T
        Private _28 As T
        Private _29 As T
        Private _30 As T
        Private _31 As T
        Private _32 As T
        Private _33 As T
        Private _34 As T
        Private _35 As T
        Private _36 As T
        Private _37 As T
        Private _38 As T
        Private _39 As T
        Private _40 As T
        Private _41 As T
        Private _42 As T
        Private _43 As T
        Private _44 As T
        Private _45 As T
        Private _46 As T
        Private _47 As T
        Private _48 As T
        Private _49 As T
        Private _50 As T
        Private _51 As T
        Private _52 As T
        Private _53 As T
        Private _54 As T
        Private _55 As T
        Private _56 As T
        Private _57 As T
        Private _58 As T
        Private _59 As T
        Private _60 As T
        Private _61 As T
        Private _62 As T
        Private _63 As T
        Private _64 As T
        Private _65 As T
        Private _66 As T
        Private _67 As T
        Private _68 As T
        Private _69 As T
        Private _70 As T
        Private _71 As T
        Private _72 As T
        Private _73 As T
        Private _74 As T
        Private _75 As T
        Private _76 As T
        Private _77 As T
        Private _78 As T
        Private _79 As T
        Private _80 As T
        Private _81 As T
        Private _82 As T
        Private _83 As T
        Private _84 As T
        Private _85 As T
        Private _86 As T
        Private _87 As T
        Private _88 As T
        Private _89 As T
        Private _90 As T
        Private _91 As T
        Private _92 As T
        Private _93 As T
        Private _94 As T
        Private _95 As T
        Private _96 As T
        Private _97 As T
        Private _98 As T
        Private _99 As T
        Private _100 As T
        Private _101 As T
        Private _102 As T
        Private _103 As T
        Private _104 As T
        Private _105 As T
        Private _106 As T
        Private _107 As T
        Private _108 As T
        Private _109 As T
        Private _110 As T
        Private _111 As T
        Private _112 As T
        Private _113 As T
        Private _114 As T
        Private _115 As T
        Private _116 As T
        Private _117 As T
        Private _118 As T
        Private _119 As T
        Private _120 As T
        Private _121 As T
        Private _122 As T
        Private _123 As T
        Private _124 As T
        Private _125 As T
        Private _126 As T
        Private _127 As T
        Private _128 As T
        Private _129 As T
        Private _130 As T
        Private _131 As T
        Private _132 As T
        Private _133 As T
        Private _134 As T
        Private _135 As T
        Private _136 As T
        Private _137 As T
        Private _138 As T
        Private _139 As T
        Private _140 As T
        Private _141 As T
        Private _142 As T
        Private _143 As T
        Private _144 As T
        Private _145 As T
        Private _146 As T
        Private _147 As T
        Private _148 As T
        Private _149 As T
        Private _150 As T
        Private _151 As T
        Private _152 As T
        Private _153 As T
        Private _154 As T
        Private _155 As T
        Private _156 As T
        Private _157 As T
        Private _158 As T
        Private _159 As T
        Private _160 As T
        Private _161 As T
        Private _162 As T
        Private _163 As T
        Private _164 As T
        Private _165 As T
        Private _166 As T
        Private _167 As T
        Private _168 As T
        Private _169 As T
        Private _170 As T
        Private _171 As T
        Private _172 As T
        Private _173 As T
        Private _174 As T
        Private _175 As T
        Private _176 As T
        Private _177 As T
        Private _178 As T
        Private _179 As T
        Private _180 As T
        Private _181 As T
        Private _182 As T
        Private _183 As T
        Private _184 As T
        Private _185 As T
        Private _186 As T
        Private _187 As T
        Private _188 As T
        Private _189 As T
        Private _190 As T
        Private _191 As T
        Private _192 As T
        Private _193 As T
        Private _194 As T
        Private _195 As T
        Private _196 As T
        Private _197 As T
        Private _198 As T
        Private _199 As T
        Private _200 As T
        Private _201 As T
        Private _202 As T
        Private _203 As T
        Private _204 As T
        Private _205 As T
        Private _206 As T
        Private _207 As T
        Private _208 As T
        Private _209 As T
        Private _210 As T
        Private _211 As T
        Private _212 As T
        Private _213 As T
        Private _214 As T
        Private _215 As T
        Private _216 As T
        Private _217 As T
        Private _218 As T
        Private _219 As T
        Private _220 As T
        Private _221 As T
        Private _222 As T
        Private _223 As T
        Private _224 As T
        Private _225 As T
        Private _226 As T
        Private _227 As T
        Private _228 As T
        Private _229 As T
        Private _230 As T
        Private _231 As T
        Private _232 As T
        Private _233 As T
        Private _234 As T
        Private _235 As T
        Private _236 As T
        Private _237 As T
        Private _238 As T
        Private _239 As T
        Private _240 As T
        Private _241 As T
        Private _242 As T
        Private _243 As T
        Private _244 As T
        Private _245 As T
        Private _246 As T
        Private _247 As T
        Private _248 As T
        Private _249 As T
        Private _250 As T
        Private _251 As T
        Private _252 As T
        Private _253 As T
        Private _254 As T
        Private _255 As T
        Private _256 As T
        Private _257 As T
        Private _258 As T
        Private _259 As T
        Private _260 As T
        Private _261 As T
        Private _262 As T
        Private _263 As T
        Private _264 As T
        Private _265 As T
        Private _266 As T
        Private _267 As T
        Private _268 As T
        Private _269 As T
        Private _270 As T
        Private _271 As T
        Private _272 As T
        Private _273 As T
        Private _274 As T
        Private _275 As T
        Private _276 As T
        Private _277 As T
        Private _278 As T
        Private _279 As T
        Private _280 As T
        Private _281 As T
        Private _282 As T
        Private _283 As T
        Private _284 As T
        Private _285 As T
        Private _286 As T
        Private _287 As T
        Private _288 As T
        Private _289 As T
        Private _290 As T
        Private _291 As T
        Private _292 As T
        Private _293 As T
        Private _294 As T
        Private _295 As T
        Private _296 As T
        Private _297 As T
        Private _298 As T
        Private _299 As T
        Private _300 As T
        Private _301 As T
        Private _302 As T
        Private _303 As T
        Private _304 As T
        Private _305 As T
        Private _306 As T
        Private _307 As T
        Private _308 As T
        Private _309 As T
        Private _310 As T
        Private _311 As T
        Private _312 As T
        Private _313 As T
        Private _314 As T
        Private _315 As T
        Private _316 As T
        Private _317 As T
        Private _318 As T
        Private _319 As T
        Private _320 As T
        Private _321 As T
        Private _322 As T
        Private _323 As T
        Private _324 As T
        Private _325 As T
        Private _326 As T
        Private _327 As T
        Private _328 As T
        Private _329 As T
        Private _330 As T
        Private _331 As T
        Private _332 As T
        Private _333 As T
        Private _334 As T
        Private _335 As T
        Private _336 As T
        Private _337 As T
        Private _338 As T
        Private _339 As T
        Private _340 As T
        Private _341 As T
        Private _342 As T
        Private _343 As T
        Private _344 As T
        Private _345 As T
        Private _346 As T
        Private _347 As T
        Private _348 As T
        Private _349 As T
        Private _350 As T
        Private _351 As T
        Private _352 As T
        Private _353 As T
        Private _354 As T
        Private _355 As T
        Private _356 As T
        Private _357 As T
        Private _358 As T
        Private _359 As T
        Private _360 As T
        Private _361 As T
        Private _362 As T
        Private _363 As T
        Private _364 As T
        Private _365 As T
        Private _366 As T
        Private _367 As T
        Private _368 As T
        Private _369 As T
        Private _370 As T
        Private _371 As T
        Private _372 As T
        Private _373 As T
        Private _374 As T
        Private _375 As T
        Private _376 As T
        Private _377 As T
        Private _378 As T
        Private _379 As T
        Private _380 As T
        Private _381 As T
        Private _382 As T
        Private _383 As T
        Private _384 As T
        Private _385 As T
        Private _386 As T
        Private _387 As T
        Private _388 As T
        Private _389 As T
        Private _390 As T
        Private _391 As T
        Private _392 As T
        Private _393 As T
        Private _394 As T
        Private _395 As T
        Private _396 As T
        Private _397 As T
        Private _398 As T
        Private _399 As T
        Private _400 As T
        Private _401 As T
        Private _402 As T
        Private _403 As T
        Private _404 As T
        Private _405 As T
        Private _406 As T
        Private _407 As T
        Private _408 As T
        Private _409 As T
        Private _410 As T
        Private _411 As T
        Private _412 As T
        Private _413 As T
        Private _414 As T
        Private _415 As T
        Private _416 As T
        Private _417 As T
        Private _418 As T
        Private _419 As T
        Private _420 As T
        Private _421 As T
        Private _422 As T
        Private _423 As T
        Private _424 As T
        Private _425 As T
        Private _426 As T
        Private _427 As T
        Private _428 As T
        Private _429 As T
        Private _430 As T
        Private _431 As T
        Private _432 As T
        Private _433 As T
        Private _434 As T
        Private _435 As T
        Private _436 As T
        Private _437 As T
        Private _438 As T
        Private _439 As T
        Private _440 As T
        Private _441 As T
        Private _442 As T
        Private _443 As T
        Private _444 As T
        Private _445 As T
        Private _446 As T
        Private _447 As T
        Private _448 As T
        Private _449 As T
        Private _450 As T
        Private _451 As T
        Private _452 As T
        Private _453 As T
        Private _454 As T
        Private _455 As T
        Private _456 As T
        Private _457 As T
        Private _458 As T
        Private _459 As T
        Private _460 As T
        Private _461 As T
        Private _462 As T
        Private _463 As T
        Private _464 As T
        Private _465 As T
        Private _466 As T
        Private _467 As T
        Private _468 As T
        Private _469 As T
        Private _470 As T
        Private _471 As T
        Private _472 As T
        Private _473 As T
        Private _474 As T
        Private _475 As T
        Private _476 As T
        Private _477 As T
        Private _478 As T
        Private _479 As T
        Private _480 As T
        Private _481 As T
        Private _482 As T
        Private _483 As T
        Private _484 As T
        Private _485 As T
        Private _486 As T
        Private _487 As T
        Private _488 As T
        Private _489 As T
        Private _490 As T
        Private _491 As T
        Private _492 As T
        Private _493 As T
        Private _494 As T
        Private _495 As T
        Private _496 As T
        Private _497 As T
        Private _498 As T
        Private _499 As T
        Private _500 As T
        Private _501 As T
        Private _502 As T
        Private _503 As T
        Private _504 As T
        Private _505 As T
        Private _506 As T
        Private _507 As T
        Private _508 As T
        Private _509 As T
        Private _510 As T
        Private _511 As T
        Private _512 As T
        Private _513 As T
        Private _514 As T
        Private _515 As T
        Private _516 As T
        Private _517 As T
        Private _518 As T
        Private _519 As T
        Private _520 As T
        Private _521 As T
        Private _522 As T
        Private _523 As T
        Private _524 As T
        Private _525 As T
        Private _526 As T
        Private _527 As T
        Private _528 As T
        Private _529 As T
        Private _530 As T
        Private _531 As T
        Private _532 As T
        Private _533 As T
        Private _534 As T
        Private _535 As T
        Private _536 As T
        Private _537 As T
        Private _538 As T
        Private _539 As T
        Private _540 As T
        Private _541 As T
        Private _542 As T
        Private _543 As T
        Private _544 As T
        Private _545 As T
        Private _546 As T
        Private _547 As T
        Private _548 As T
        Private _549 As T
        Private _550 As T
        Private _551 As T
        Private _552 As T
        Private _553 As T
        Private _554 As T
        Private _555 As T
        Private _556 As T
        Private _557 As T
        Private _558 As T
        Private _559 As T
        Private _560 As T
        Private _561 As T
        Private _562 As T
        Private _563 As T
        Private _564 As T
        Private _565 As T
        Private _566 As T
        Private _567 As T
        Private _568 As T
        Private _569 As T
        Private _570 As T
        Private _571 As T
        Private _572 As T
        Private _573 As T
        Private _574 As T
        Private _575 As T
        Private _576 As T
        Private _577 As T
        Private _578 As T
        Private _579 As T
        Private _580 As T
        Private _581 As T
        Private _582 As T
        Private _583 As T
        Private _584 As T
        Private _585 As T
        Private _586 As T
        Private _587 As T
        Private _588 As T
        Private _589 As T
        Private _590 As T
        Private _591 As T
        Private _592 As T
        Private _593 As T
        Private _594 As T
        Private _595 As T
        Private _596 As T
        Private _597 As T
        Private _598 As T
        Private _599 As T
        Private _600 As T
        Private _601 As T
        Private _602 As T
        Private _603 As T
        Private _604 As T
        Private _605 As T
        Private _606 As T
        Private _607 As T
        Private _608 As T
        Private _609 As T
        Private _610 As T
        Private _611 As T
        Private _612 As T
        Private _613 As T
        Private _614 As T
        Private _615 As T
        Private _616 As T
        Private _617 As T
        Private _618 As T
        Private _619 As T
        Private _620 As T
        Private _621 As T
        Private _622 As T
        Private _623 As T
        Private _624 As T
        Private _625 As T
        Private _626 As T
        Private _627 As T
        Private _628 As T
        Private _629 As T
        Private _630 As T
        Private _631 As T
        Private _632 As T
        Private _633 As T
        Private _634 As T
        Private _635 As T
        Private _636 As T
        Private _637 As T
        Private _638 As T
        Private _639 As T
        Private _640 As T
        Private _641 As T
        Private _642 As T
        Private _643 As T
        Private _644 As T
        Private _645 As T
        Private _646 As T
        Private _647 As T
        Private _648 As T
        Private _649 As T
        Private _650 As T
        Private _651 As T
        Private _652 As T
        Private _653 As T
        Private _654 As T
        Private _655 As T
        Private _656 As T
        Private _657 As T
        Private _658 As T
        Private _659 As T
        Private _660 As T
        Private _661 As T
        Private _662 As T
        Private _663 As T
        Private _664 As T
        Private _665 As T
        Private _666 As T
        Private _667 As T
        Private _668 As T
        Private _669 As T
        Private _670 As T
        Private _671 As T
        Private _672 As T
        Private _673 As T
        Private _674 As T
        Private _675 As T
        Private _676 As T
        Private _677 As T
        Private _678 As T
        Private _679 As T
        Private _680 As T
        Private _681 As T
        Private _682 As T
        Private _683 As T
        Private _684 As T
        Private _685 As T
        Private _686 As T
        Private _687 As T
        Private _688 As T
        Private _689 As T
        Private _690 As T
        Private _691 As T
        Private _692 As T
        Private _693 As T
        Private _694 As T
        Private _695 As T
        Private _696 As T
        Private _697 As T
        Private _698 As T
        Private _699 As T
        Private _700 As T
        Private _701 As T
        Private _702 As T
        Private _703 As T
        Private _704 As T
        Private _705 As T
        Private _706 As T
        Private _707 As T
        Private _708 As T
        Private _709 As T
        Private _710 As T
        Private _711 As T
        Private _712 As T
        Private _713 As T
        Private _714 As T
        Private _715 As T
        Private _716 As T
        Private _717 As T
        Private _718 As T
        Private _719 As T
        Private _720 As T
        Private _721 As T
        Private _722 As T
        Private _723 As T
        Private _724 As T
        Private _725 As T
        Private _726 As T
        Private _727 As T
        Private _728 As T
        Private _729 As T
        Private _730 As T
        Private _731 As T
        Private _732 As T
        Private _733 As T
        Private _734 As T
        Private _735 As T
        Private _736 As T
        Private _737 As T
        Private _738 As T
        Private _739 As T
        Private _740 As T
        Private _741 As T
        Private _742 As T
        Private _743 As T
        Private _744 As T
        Private _745 As T
        Private _746 As T
        Private _747 As T
        Private _748 As T
        Private _749 As T
        Private _750 As T
        Private _751 As T
        Private _752 As T
        Private _753 As T
        Private _754 As T
        Private _755 As T
        Private _756 As T
        Private _757 As T
        Private _758 As T
        Private _759 As T
        Private _760 As T
        Private _761 As T
        Private _762 As T
        Private _763 As T
        Private _764 As T
        Private _765 As T
        Private _766 As T
        Private _767 As T
        Private _768 As T
        Private _769 As T
        Private _770 As T
        Private _771 As T
        Private _772 As T
        Private _773 As T
        Private _774 As T
        Private _775 As T
        Private _776 As T
        Private _777 As T
        Private _778 As T
        Private _779 As T
        Private _780 As T
        Private _781 As T
        Private _782 As T
        Private _783 As T
        Private _784 As T
        Private _785 As T
        Private _786 As T
        Private _787 As T
        Private _788 As T
        Private _789 As T
        Private _790 As T
        Private _791 As T
        Private _792 As T
        Private _793 As T
        Private _794 As T
        Private _795 As T
        Private _796 As T
        Private _797 As T
        Private _798 As T
        Private _799 As T
        Private _800 As T
        Private _801 As T
        Private _802 As T
        Private _803 As T
        Private _804 As T
        Private _805 As T
        Private _806 As T
        Private _807 As T
        Private _808 As T
        Private _809 As T
        Private _810 As T
        Private _811 As T
        Private _812 As T
        Private _813 As T
        Private _814 As T
        Private _815 As T
        Private _816 As T
        Private _817 As T
        Private _818 As T
        Private _819 As T
        Private _820 As T
        Private _821 As T
        Private _822 As T
        Private _823 As T
        Private _824 As T
        Private _825 As T
        Private _826 As T
        Private _827 As T
        Private _828 As T
        Private _829 As T
        Private _830 As T
        Private _831 As T
        Private _832 As T
        Private _833 As T
        Private _834 As T
        Private _835 As T
        Private _836 As T
        Private _837 As T
        Private _838 As T
        Private _839 As T
        Private _840 As T
        Private _841 As T
        Private _842 As T
        Private _843 As T
        Private _844 As T
        Private _845 As T
        Private _846 As T
        Private _847 As T
        Private _848 As T
        Private _849 As T
        Private _850 As T
        Private _851 As T
        Private _852 As T
        Private _853 As T
        Private _854 As T
        Private _855 As T
        Private _856 As T
        Private _857 As T
        Private _858 As T
        Private _859 As T
        Private _860 As T
        Private _861 As T
        Private _862 As T
        Private _863 As T
        Private _864 As T
        Private _865 As T
        Private _866 As T
        Private _867 As T
        Private _868 As T
        Private _869 As T
        Private _870 As T
        Private _871 As T
        Private _872 As T
        Private _873 As T
        Private _874 As T
        Private _875 As T
        Private _876 As T
        Private _877 As T
        Private _878 As T
        Private _879 As T
        Private _880 As T
        Private _881 As T
        Private _882 As T
        Private _883 As T
        Private _884 As T
        Private _885 As T
        Private _886 As T
        Private _887 As T
        Private _888 As T
        Private _889 As T
        Private _890 As T
        Private _891 As T
        Private _892 As T
        Private _893 As T
        Private _894 As T
        Private _895 As T
        Private _896 As T
        Private _897 As T
        Private _898 As T
        Private _899 As T
        Private _900 As T
        Private _901 As T
        Private _902 As T
        Private _903 As T
        Private _904 As T
        Private _905 As T
        Private _906 As T
        Private _907 As T
        Private _908 As T
        Private _909 As T
        Private _910 As T
        Private _911 As T
        Private _912 As T
        Private _913 As T
        Private _914 As T
        Private _915 As T
        Private _916 As T
        Private _917 As T
        Private _918 As T
        Private _919 As T
        Private _920 As T
        Private _921 As T
        Private _922 As T
        Private _923 As T
        Private _924 As T
        Private _925 As T
        Private _926 As T
        Private _927 As T
        Private _928 As T
        Private _929 As T
        Private _930 As T
        Private _931 As T
        Private _932 As T
        Private _933 As T
        Private _934 As T
        Private _935 As T
        Private _936 As T
        Private _937 As T
        Private _938 As T
        Private _939 As T
        Private _940 As T
        Private _941 As T
        Private _942 As T
        Private _943 As T
        Private _944 As T
        Private _945 As T
        Private _946 As T
        Private _947 As T
        Private _948 As T
        Private _949 As T
        Private _950 As T
        Private _951 As T
        Private _952 As T
        Private _953 As T
        Private _954 As T
        Private _955 As T
        Private _956 As T
        Private _957 As T
        Private _958 As T
        Private _959 As T
        Private _960 As T
        Private _961 As T
        Private _962 As T
        Private _963 As T
        Private _964 As T
        Private _965 As T
        Private _966 As T
        Private _967 As T
        Private _968 As T
        Private _969 As T
        Private _970 As T
        Private _971 As T
        Private _972 As T
        Private _973 As T
        Private _974 As T
        Private _975 As T
        Private _976 As T
        Private _977 As T
        Private _978 As T
        Private _979 As T
        Private _980 As T
        Private _981 As T
        Private _982 As T
        Private _983 As T
        Private _984 As T
        Private _985 As T
        Private _986 As T
        Private _987 As T
        Private _988 As T
        Private _989 As T
        Private _990 As T
        Private _991 As T
        Private _992 As T
        Private _993 As T
        Private _994 As T
        Private _995 As T
        Private _996 As T
        Private _997 As T
        Private _998 As T
        Private _999 As T
        Private _1000 As T
        Private _1001 As T
        Private _1002 As T
        Private _1003 As T
        Private _1004 As T
        Private _1005 As T
        Private _1006 As T
        Private _1007 As T
        Private _1008 As T
        Private _1009 As T
        Private _1010 As T
        Private _1011 As T
        Private _1012 As T
        Private _1013 As T
        Private _1014 As T
        Private _1015 As T
        Private _1016 As T
        Private _1017 As T
        Private _1018 As T
        Private _1019 As T
        Private _1020 As T
        Private _1021 As T
        Private _1022 As T
        Private _1023 As T
        ''' <summary>
        ''' The maximum length of the current buffer.
        ''' </summary>
        Public Const MaxCount = 1024

        ''' <summary>
        ''' Gets or sets the specified item at the specified index.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        Default Public Property Item(index As Integer) As T
            Get
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Return Unsafe.Add(_0, index)
            End Get
            Set(value As T)
                If index < 0 OrElse index >= MaxCount Then Throw New ArgumentOutOfRangeException()
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the specified item at the specified index without boundary check.
        ''' </summary>
        ''' <param name="index">The index of the element.</param>
        ''' <value>The element at the specified index.</value>
        <EditorBrowsable(EditorBrowsableState.Advanced)>
        Public Property UnsafeItem(index As Integer) As T
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Unsafe.Add(_0, index)
            End Get
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Set(value As T)
                Unsafe.Add(_0, index) = value
            End Set
        End Property

        ''' <summary>
        ''' Gets a new value at the specified index. Returns whether the element was got.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be acquired.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TryGetItem(index As Integer, ByRef value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            value = Unsafe.Add(_0, index)
            Return True
        End Function

        ''' <summary>
        ''' Sets a new value at the specified index. Returns whether the element was set.
        ''' </summary>
        ''' <param name="index">The index of the new element.</param>
        ''' <param name="value">The new value to be set.</param>
        ''' <returns>If returns <see langword="True"/>, the item was set successfully. Otherwise, the item can't be set.</returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function TrySetItem(index As Integer, value As T) As Boolean
            If index < 0 OrElse index >= MaxCount Then Return False
            Unsafe.Add(_0, index) = value
            Return True
        End Function
    End Structure
End Namespace
