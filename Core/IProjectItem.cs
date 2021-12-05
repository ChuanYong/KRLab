// KRLab - Free class diagram editor
 

using System;
using System.Xml;

namespace KRLab.Core
{
	public interface IProjectItem : IModifiable
	{
		event EventHandler Renamed;
		event EventHandler Closing;

		string Name { get; }

		Project Project { get; set; } 

		bool IsUntitled { get; }


		void Close();

        /// <summary>
        /// 根据选择的文件类型检查并保存文件
        /// </summary>
        /// <param name="node"></param>
		void Serialize(XmlElement node);

		/// <exception cref="InvalidDataException">
		/// The serialized format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		void Deserialize(XmlElement node);
	}
}
