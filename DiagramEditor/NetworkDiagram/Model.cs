using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using KRLab.Translations;

using KRLab.Core;
using KRLab.Core.SNet;
using Utilities;
 

namespace KRLab.DiagramEditor.NetworkDiagram
{ 
	public class Model : IProjectItem
	{
		private string _Name;
        private KnowledgeNet _KnowledgeNet;

		private List<IEntity> entities = new List<IEntity>();
		private List<Relationship> relationships = new List<Relationship>();
		private Project project = null;

		private bool isDirty = false;
		private bool loading = false;

		public event EventHandler Modified;
		public event EventHandler Renamed;
		public event EventHandler Closing;
		public event EntityEventHandler EntityAdded;
		public event EntityEventHandler EntityRemoved;
		public event RelationshipEventHandler RelationAdded;
		public event RelationshipEventHandler RelationRemoved;
		public event SerializeEventHandler Serializing;
		public event SerializeEventHandler Deserializing;

        protected Model()
        {
            this.Name = Strings.Untitled;
            _KnowledgeNet = null;
        }

        public Model(KnowledgeNet knowledge):this(null,knowledge)
        { 
        }

		public Model(string name,KnowledgeNet knowledge )
		{
            if (knowledge == null)
            {
                throw new ArgumentNullException("KnowledgeNet");
            }
			if (name != null && name.Length == 0)
				throw new ArgumentException("Name cannot empty string.");
            _KnowledgeNet = knowledge;
			this.Name = name;
		}

        /// <summary>
        /// 该函数在DiagramNode的LabelModified()函数中调用
        /// </summary>
		public string Name
		{
			get
			{
				if (_Name == null)
					return Strings.Untitled;
				else
                    return _Name;
			}
			set
			{
                if (_Name != value && value != null)
				{
                    _Name = value;  
					OnRenamed(EventArgs.Empty);
					OnModified(EventArgs.Empty);
				}
			}
		}
         

		public Project Project
		{
			get { return project; }
			set { project = value; }
		}

		public bool IsUntitled
		{
			get
			{
                return (_Name == null);
			}
		}

		public bool IsDirty
		{
			get { return isDirty; }
		}

		protected bool Loading
		{
			get { return loading; }
		}

		public bool IsEmpty
		{
			get
			{
				return (entities.Count == 0 && relationships.Count == 0);
			}
		}

        public KnowledgeNet KnowledgeNet
        {
            get { return _KnowledgeNet; }
        }

		void IModifiable.Clean()
		{
			isDirty = false;
			//TODO: tagokat is tisztítani!
		}

		void IProjectItem.Close()
		{
			OnClosing(EventArgs.Empty);
		}

		public IEnumerable<IEntity> Entities
		{
			get { return entities; }
		}

		public IEnumerable<Relationship> Relationships
		{
			get { return relationships; }
		}

		private void ElementChanged(object sender, EventArgs e)
		{
			OnModified(e);
		}

		private void AddEntity(IEntity entity)
		{
			entities.Add(entity);
			entity.Modified += new EventHandler(ElementChanged);
			OnEntityAdded(new EntityEventArgs(entity));
		}

        public NodeBase AddNode()
        {
            NodeBase newNode = KnowledgeNet.CreateNode();
            AddNode(newNode);
            return newNode;
        } 

        protected virtual void AddNode(NodeBase newNode)
        {
            AddEntity(newNode);
        }

		private void AddRelationship(Relationship relationship)
		{
			relationships.Add(relationship);
			relationship.Modified += new EventHandler(ElementChanged);
			OnRelationAdded(new RelationshipEventArgs(relationship));
		} 


		public void RemoveEntity(IEntity entity)
		{
			if (entities.Remove(entity))
			{
				entity.Modified -= new EventHandler(ElementChanged);
				RemoveRelationships(entity);
				OnEntityRemoved(new EntityEventArgs(entity));
			}
		}

		private void RemoveRelationships(IEntity entity)
		{
			for (int i = 0; i < relationships.Count; i++)
			{
				Relationship relationship = relationships[i];
				if (relationship.First == entity || relationship.Second == entity)
				{
					relationship.Detach();
					relationship.Modified -= new EventHandler(ElementChanged);
					relationships.RemoveAt(i--);
					OnRelationRemoved(new RelationshipEventArgs(relationship));
				}
			}
		}

		public void RemoveRelationship(Relationship relationship)
		{
			if (relationships.Contains(relationship))
			{
				relationship.Detach();
				relationship.Modified -= new EventHandler(ElementChanged);
				relationships.Remove(relationship);
				OnRelationRemoved(new RelationshipEventArgs(relationship));
			}
		}

		void IProjectItem.Serialize(XmlElement node)
		{
			Serialize(node);
		}

		void IProjectItem.Deserialize(XmlElement node)
		{
			Deserialize(node);
		}


        /// <summary>
        /// 根据文件类型检查并保存文件
        /// </summary>
        /// <param name="node"></param> 
        private void Serialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("root");

			XmlElement nameElement = node.OwnerDocument.CreateElement("Name");
			nameElement.InnerText = Name;
			node.AppendChild(nameElement);

            XmlElement netElement = node.OwnerDocument.CreateElement("KnowledgeNet");
            netElement.InnerText = KnowledgeNet.Name;
            node.AppendChild(netElement);

            if (KnowledgeNet.Type == NetGraphType.SemanticNet)
            {
                SaveSNEntitites(node);
                SaveSNRelationships(node);
                
            }
            else if (KnowledgeNet.Type == NetGraphType.BayesianNet)
            {
                throw new NotImplementedException(); 
            }
            else if (KnowledgeNet.Type == NetGraphType.ConceptMap)
            {
                throw new NotImplementedException();
            }
            else
                throw new Exception("没有定制这种知识类型的图");

			OnSerializing(new SerializeEventArgs(node));
		}

		/// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		private void Deserialize(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("root");
			loading = true;

			XmlElement nameElement = node["Name"];
			if (nameElement == null || nameElement.InnerText == "")
				_Name = null;
			else
                _Name = nameElement.InnerText;

            XmlElement netElement = node["KnowledgeNet"];
            string knowledgeName = netElement.InnerText;
            if (knowledgeName == SemanticNetTemplate.Instance.Name)
            {
                _KnowledgeNet = SemanticNetTemplate.Instance;
            }
            else if (knowledgeName == BayesianNetTemplate.Instance.Name)
            {
                _KnowledgeNet = BayesianNetTemplate.Instance;
            }
            else
            {
                _KnowledgeNet = ConceptMapTemplate.Instance;
            }

            if(_KnowledgeNet.Type==NetGraphType.SemanticNet)
            {
			    LoadSNEntitites(node);
			    LoadSNRelationships(node);
            }

			OnDeserializing(new SerializeEventArgs(node));
			loading = false;
		}

		/// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> is null.
		/// </exception>
		private void LoadSNEntitites(XmlNode root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			XmlNodeList nodeList = root.SelectNodes("Entities/Entity");

			foreach (XmlElement node in nodeList)
			{
				try
				{
					string type = node.GetAttribute("type");

					IEntity entity = GetEntity(type);
					entity.Deserialize(node);
				}
				catch (BadSyntaxException ex)
				{
					throw new InvalidDataException("Invalid entity.", ex);
				}
			}
		}

		private IEntity GetEntity(string type)
		{
			switch (type)
			{
                case "SemanticNode":
                case "ConceptNode":
                case "BeyasianNode":
                    return AddNode();
				case "Comment":
					return AddComment();

				default:                    
					throw new InvalidDataException("Invalid entity type: " + type);
			}
		}

        public Comment AddComment()
        {
            Comment comment = new Comment();
            AddComment(comment);
            return comment;
        }

        protected virtual void AddComment(Comment comment)
        {
            AddEntity(comment);
        }

        public bool InsertComment(Comment comment)
        {
            if (comment != null && !entities.Contains(comment))
            {
                AddComment(comment);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool InsertNode(NodeBase newNode)
        {
            if (newNode != null && !entities.Contains(newNode))
            {
                AddNode(newNode);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="comment"/> or <paramref name="entity"/> is null.
        /// </exception>
        public virtual CommentRelation AddCommentRelationship(Comment comment, IEntity entity)
        {
            CommentRelation commentRelationship = new CommentRelation(comment, entity);

            AddCommentRelationship(commentRelationship);
            return commentRelationship;
        }

        protected virtual void AddCommentRelationship(CommentRelation commentRelationship)
        {
            AddRelationship(commentRelationship);
        }

        public bool InsertCommentRelationship(CommentRelation commentRelationship)
        {
            if (commentRelationship != null && !relationships.Contains(commentRelationship) &&
                entities.Contains(commentRelationship.First) && entities.Contains(commentRelationship.Second))
            {
                AddCommentRelationship(commentRelationship);
                return true;
            }
            else
            {
                return false;
            }
        }
         

        public bool InsertSNRelationship(SNRelationship relation)
        {
            if (relation != null && !relationships.Contains(relation) &&
                entities.Contains(relation.First) && entities.Contains(relation.Second))
            {
                AddSNRelationship(relation);
                return true;
            }
            else
            {
                return false;
            }
        } 
         
        protected virtual void AddSNRelationship(SNRelationship relation)
        {
            AddRelationship(relation);
        } 
      
        public SNRelationship AddSNRelationship(NodeBase first, NodeBase second)
        {
            SNRelationship relation = new SNRelationship((BasicSemanticNode)first, (BasicSemanticNode)second);
            AddSNRelationship(relation);
            return relation;
        }   

		/// <exception cref="InvalidDataException">
		/// The save format is corrupt and could not be loaded.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> is null.
		/// </exception>
		private void LoadSNRelationships(XmlNode root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			XmlNodeList nodeList = root.SelectNodes(
				"Relationships/Relationship|Relations/Relation"); // old file format

			foreach (XmlElement node in nodeList)
			{
				string type = node.GetAttribute("type");
                if (type != RelationshipType.SN_REL.ToString())
                    throw new Exception("不是语义网！");

				string firstString = node.GetAttribute("first");
				string secondString = node.GetAttribute("second");
				int firstIndex, secondIndex;

				if (!int.TryParse(firstString, out firstIndex) ||
					!int.TryParse(secondString, out secondIndex))
				{
					throw new InvalidDataException(Strings.ErrorCorruptSaveFormat);
				}
				if (firstIndex < 0 || firstIndex >= entities.Count ||
					secondIndex < 0 || secondIndex >= entities.Count)
				{
					throw new InvalidDataException(Strings.ErrorCorruptSaveFormat);
				}

				try
				{
					IEntity first = entities[firstIndex];
					IEntity second = entities[secondIndex];
					SNRelationship relationship=AddSNRelationship(first as BasicSemanticNode,
                            second as BasicSemanticNode);
 
					relationship.Deserialize(node);
				}
				catch (ArgumentNullException ex)
				{
					throw new InvalidDataException("Invalid relationship.", ex);
				}
				catch (RelationshipException ex)
				{
					throw new InvalidDataException("Invalid relationship.", ex);
				}
			}
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="node"/> is null.
		/// </exception>
		private void SaveSNEntitites(XmlElement node)
		{
			if (node == null)
				throw new ArgumentNullException("root");

			XmlElement entitiesChild = node.OwnerDocument.CreateElement("Entities");

			foreach (IEntity entity in entities)
			{
				XmlElement child = node.OwnerDocument.CreateElement("Entity");

				entity.Serialize(child);
				child.SetAttribute("type", entity.EntityType.ToString());
				entitiesChild.AppendChild(child);
			}
			node.AppendChild(entitiesChild);
		}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="root"/> is null.
		/// </exception>
		private void SaveSNRelationships(XmlNode root)
		{
			if (root == null)
				throw new ArgumentNullException("root");

			XmlElement relationsChild = root.OwnerDocument.CreateElement("Relationships");

			foreach (Relationship relationship in relationships)
			{
                SNRelationship snRel = relationship as SNRelationship;
                if (snRel == null)
                    throw new Exception("不是语义连接！");

				XmlElement child = root.OwnerDocument.CreateElement("Relationship");

				int firstIndex = entities.IndexOf(relationship.First);
				int secondIndex = entities.IndexOf(relationship.Second);

				relationship.Serialize(child);
				child.SetAttribute("type", relationship.RelationshipType.ToString()); 
				child.SetAttribute("first", firstIndex.ToString());
				child.SetAttribute("second", secondIndex.ToString());

				relationsChild.AppendChild(child);
			}
			root.AppendChild(relationsChild);
		}

		protected virtual void OnEntityAdded(EntityEventArgs e)
		{
			if (EntityAdded != null)
				EntityAdded(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnEntityRemoved(EntityEventArgs e)
		{
			if (EntityRemoved != null)
				EntityRemoved(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnRelationAdded(RelationshipEventArgs e)
		{
			if (RelationAdded != null)
				RelationAdded(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnRelationRemoved(RelationshipEventArgs e)
		{
			if (RelationRemoved != null)
				RelationRemoved(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnSerializing(SerializeEventArgs e)
		{
			if (Serializing != null)
				Serializing(this, e);
		}

		protected virtual void OnDeserializing(SerializeEventArgs e)
		{
			if (Deserializing != null)
				Deserializing(this, e);
			OnModified(EventArgs.Empty);
		}

		protected virtual void OnModified(EventArgs e)
		{
			isDirty = true;
			if (Modified != null)
				Modified(this, e);
		}

		protected virtual void OnRenamed(EventArgs e)
		{
            if (Renamed != null)
            {
                Renamed(this, e); 
            }
		}

		protected virtual void OnClosing(EventArgs e)
		{
			if (Closing != null)
				Closing(this, e);
		}

		public override string ToString()
		{
			if (IsDirty)
				return Name + "*";
			else
				return Name;
		}
	}
}
