using GameProject.Core;
using GameProject.Rooms.Definitions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Rooms.Loading;

/// <summary>Stores and updates the objects in one room.</summary>
public sealed class LoadedRoom : IGameObject
{
    public LoadedRoom(RoomDefinition definition, IRoomObjectFactory factory)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(factory);
        RoomValidator.Validate(definition);
        Definition = definition;
        Dictionary<string, IGameObject> objects = new(StringComparer.Ordinal);
        foreach (RoomObjectDefinition obj in definition.Objects)
            objects.Add(obj.Id, factory.Create(obj) ?? throw new InvalidOperationException($"Factory returned null for '{obj.Id}'."));
        Objects = new System.Collections.ObjectModel.ReadOnlyDictionary<string, IGameObject>(objects);
    }

    public RoomDefinition Definition { get; }
    public string Name => Definition.Name;
    public IReadOnlyDictionary<string, IGameObject> Objects { get; }

    public void Update(GameTime gameTime)
    {
        ArgumentNullException.ThrowIfNull(gameTime);
        foreach (IGameObject obj in Objects.Values) obj.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (IGameObject obj in Objects.Values) obj.Draw(spriteBatch);
    }

    public void Reset()
    {
        foreach (IGameObject obj in Objects.Values) obj.Reset();
    }
}
