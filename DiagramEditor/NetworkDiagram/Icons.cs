 

using System;
using System.Drawing;
using System.Windows.Forms;
using KRLab.Core;
using System.Drawing.Imaging;

namespace KRLab.DiagramEditor.NetworkDiagram
{
	public static class Icons
	{
		const int DefaultDestructorIndex = 84;
		const int PrivateDestructorIndex = 85;
		public const int InterfaceImageIndex = 86;
		public const int EnumItemImageIndex = 87;
		public const int ParameterImageIndex = 88;
		public const int ClassImageIndex = 89;

		static Bitmap[] images;
		static ImageList imageList;

		static Icons()
		{
			LoadImages();
		}

		public static ImageList IconList
		{
			get
			{
				return imageList;
			}
		}

		private static void LoadImages()
		{
			images = new Bitmap[] {
				Properties.Resources.DefaultConst,
				Properties.Resources.PublicConst,
				Properties.Resources.ProtintConst,
				Properties.Resources.InternalConst,
				Properties.Resources.ProtectedConst,
				Properties.Resources.PrivateConst,

				Properties.Resources.DefaultField,
				Properties.Resources.PublicField,
				Properties.Resources.ProtintField,
				Properties.Resources.InternalField,
				Properties.Resources.ProtectedField,
				Properties.Resources.PrivateField,

				Properties.Resources.DefaultConstructor,
				Properties.Resources.PublicConstructor,
				Properties.Resources.ProtintConstructor,
				Properties.Resources.InternalConstructor, 
				Properties.Resources.PrivateConstructor,

				Properties.Resources.DefaultOperator,
				Properties.Resources.PublicOperator,
				Properties.Resources.ProtintOperator,
				Properties.Resources.InternalOperator,
				Properties.Resources.ProtectedOperator,
				Properties.Resources.PrivateOperator,

				Properties.Resources.DefaultMethod,
				Properties.Resources.PublicMethod,
				Properties.Resources.ProtintMethod,
				Properties.Resources.InternalMethod,
				Properties.Resources.ProtectedMethod, 

				Properties.Resources.DefaultReadonly,
				Properties.Resources.PublicReadonly,
				Properties.Resources.ProtintReadonly,
				Properties.Resources.InternalReadonly,
				Properties.Resources.ProtectedReadonly,
				Properties.Resources.PrivateReadonly,

				Properties.Resources.DefaultWriteonly, 
				Properties.Resources.ProtintWriteonly,
				Properties.Resources.InternalWriteoly,
				Properties.Resources.ProtectedWriteonly,
				Properties.Resources.PrivateWriteonly,

				Properties.Resources.DefaultProperty,
				Properties.Resources.PublicProperty,
				Properties.Resources.ProtintProperty,
				Properties.Resources.InternalProperty,
				Properties.Resources.ProtectedProperty,
				Properties.Resources.PrivateProperty,

				Properties.Resources.DefaultEvent,
				Properties.Resources.PublicEvent,
				Properties.Resources.ProtintEvent,
				Properties.Resources.InternalEvent,
				Properties.Resources.ProtectedEvent,
				Properties.Resources.PrivateEvent,

				Properties.Resources.DefaultClass,
				Properties.Resources.PublicClass,
				Properties.Resources.ProtintClass,
				Properties.Resources.InternalClass,
				Properties.Resources.ProtectedClass,
				Properties.Resources.PrivateClass,

				Properties.Resources.DefaultStructure,
				Properties.Resources.PublicStructure,
				Properties.Resources.ProtintStructure,
				Properties.Resources.InternalStructure,
				Properties.Resources.ProtectedStructure,
				Properties.Resources.PrivateStructure,

				Properties.Resources.DefaultInterface,
				Properties.Resources.PublicInterface,
				Properties.Resources.ProtintInterface,
				Properties.Resources.InternalInterface,
				Properties.Resources.ProtectedInterface,
				Properties.Resources.PrivateInterface,

				Properties.Resources.DefaultEnum,
				Properties.Resources.PublicEnum,
				Properties.Resources.ProtintEnum,
				Properties.Resources.InternalEnum,
				Properties.Resources.ProtectedEnum, 

				Properties.Resources.DefaultDelegate,
				Properties.Resources.PublicDelegate,
				Properties.Resources.ProtintDelegate,
				Properties.Resources.InternalDelegate,
				Properties.Resources.ProtectedDelegate,
				Properties.Resources.PrivateDelegate,

				Properties.Resources.DefaultDestructor, // 84.
				Properties.Resources.PrivateDestructor, // 85.
				Properties.Resources.Interface24,       // 86. 
				Properties.Resources.Parameter,         // 88.
				Properties.Resources.SNNode,              // 89.
                Properties.Resources.BNNode,
                Properties.Resources.CMNode,
                
                
			};

			imageList = new ImageList();
			imageList.ColorDepth = ColorDepth.Depth32Bit;
			imageList.Images.AddRange(images);
		}

		/// <exception cref="ArgumentNullException">
		/// A <paramref name="member"/> nem lehet null.
		/// </exception>
        //public static int GetImageIndex(Member member)
        //{
        //    if (member == null)
        //        throw new ArgumentNullException("member");

        //    int group = 0;

        //    if (member is Field)
        //    {
        //        if (((Field) member).IsConstant)
        //        {
        //            group = 0;
        //        }
        //        else
        //        {
        //            group = 1;
        //        }
        //    }
        //    else if (member is Method)
        //    {
        //        if (member is Destructor)
        //        {
        //            return PrivateDestructorIndex;
        //        }
        //        else if (member is Constructor)
        //        {
        //            group = 2;
        //        }
        //        else if (((Method) member).IsOperator)
        //        {
        //            group = 3;
        //        }
        //        else
        //        {
        //            group = 4;
        //        }
        //    }
        //    else if (member is Property)
        //    {
        //        Property property = (Property) member;

        //        if (property.IsReadonly)
        //        {
        //            group = 5;
        //        }
        //        else if (property.IsWriteonly)
        //        {
        //            group = 6;
        //        }
        //        else
        //        {
        //            group = 7;
        //        }
        //    }
        //    else if (member is Event)
        //    {
        //        group = 8;
        //    }

        //    return group * 6 + (int) member.Access;
        //}

        ///// <exception cref="ArgumentNullException">
        ///// <paramref name="member"/> is null.
        ///// </exception>
        //public static Image GetImage(Member member)
        //{
        //    int imageIndex = GetImageIndex(member);
        //    return images[imageIndex];
        //}

        //public static Image GetImage(MemberType type, AccessModifier access)
        //{
        //    if (type == MemberType.Destructor)
        //    {
        //        if (access == AccessModifier.Default)
        //            return Properties.Resources.DefaultDestructor;
        //        else
        //            return Properties.Resources.PrivateDestructor;
        //    }

        //    int group = 0;
        //    switch (type)
        //    {
        //        case MemberType.Field:
        //            group = 1;
        //            break;

        //        case MemberType.Method:
        //            group = 4;
        //            break;

        //        case MemberType.Constructor:
        //            group = 2;
        //            break;

        //        case MemberType.Property:
        //            group = 7;
        //            break;

        //        case MemberType.Event:
        //            group = 8;
        //            break;
        //    }

        //    return images[group * 6 + (int) access];
        //}

        ///// <exception cref="ArgumentNullException">
        ///// <paramref name="type"/> is null.
        ///// </exception>
        //public static Image GetImage(NodeBase type)
        //{
        //    if (type == null)
        //        throw new ArgumentNullException("type");

        //    return GetImage(type.EntityType, type.AccessModifier);
        //}

		/// <exception cref="ArgumentNullException">
		/// <paramref name="type"/> is null.
		/// </exception>
        //public static Image GetImage(EntityType type, AccessModifier access)
        //{
        //    int group = 0;
        //    switch (type)
        //    {
        //        case EntityType.Class:
        //            group = 9;
        //            break;

        //        case EntityType.Structure:
        //            group = 10;
        //            break;

        //        case EntityType.Interface:
        //            group = 11;
        //            break;

        //        case EntityType.Enum:
        //            group = 12;
        //            break;

        //        case EntityType.Delegate:
        //            group = 13;
        //            break;
        //    }

        //    return images[group * 6 + (int) access];
        //}
	}
}