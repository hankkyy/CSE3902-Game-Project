namespace GameProject.Core;

/// <summary>Cycles through one visible object at a time, wrapping at both ends.</summary>
public sealed class ObjectGallery<T> where T : IGameObject
{
    private readonly IReadOnlyList<T> objects;

    public ObjectGallery(IReadOnlyList<T> objects)
    {
        if (objects.Count == 0) throw new ArgumentException("A gallery needs at least one object.", nameof(objects));
        this.objects = objects;
    }

    public T Current => objects[Index];
    public int Count => objects.Count;
    public int Index { get; private set; }
    public void Next() => Index = (Index + 1) % objects.Count;
    public void Previous() => Index = (Index - 1 + objects.Count) % objects.Count;

    public void Reset()
    {
        Index = 0;
        foreach (T item in objects) item.Reset();
    }
}
