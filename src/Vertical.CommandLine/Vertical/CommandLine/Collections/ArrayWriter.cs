namespace Vertical.CommandLine.Collections;

internal ref struct ArrayWriter<T>
{
    private readonly T[] _buffer;
    private int _position;
    
    public ArrayWriter(int capacity)
    {
        _buffer = new T[capacity];
        _position = 0;
    }

    public void Add(T value) => _buffer[_position++] = value;

    public T[] ToArray() => _buffer;

    public int WrittenCount => _position;
}