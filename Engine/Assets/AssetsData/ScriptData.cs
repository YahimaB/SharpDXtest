using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Engine.BaseAssets.Components;

namespace Engine.AssetsData
{
    public class ScriptData : AssetData
    {
        public List<Type> ClassTypes = new List<Type>();

        public override void Serialize(BinaryWriter writer)
        {
            YamlManager.SaveToStream(writer.BaseStream, this);
        }

        public override void Deserialize(BinaryReader reader)
        {
            YamlManager.LoadFromStream(reader.BaseStream, this);
        }

        public override ScriptAsset ToRealAsset()
        {
            Type componentType = ClassTypes.FirstOrDefault(t => t.IsSubclassOf(typeof(Component)));
            return new ScriptAsset(componentType);
        }
    }
}