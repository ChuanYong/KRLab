using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KRLab.Core
{
    public abstract class CompositeNode:NodeBase
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                if (value != _Name)
                {
                    _Name = value;
                    Changed();
                }
            }
        }

        public abstract int MemberCount
        {
            get;
        }

        public CompositeNode()
            : this("NewCompositeNode")
        {
        }

        public CompositeNode(string name)
            : base(name)
        {
        }


        //public void SortMembers(SortingMode sortingMode)
        //{
        //    //switch (sortingMode)
        //    //{
        //    //    //case SortingMode.ByName:
        //    //    //    FieldList.Sort(MemberComparisonByName);
        //    //    //    OperationList.Sort(MemberComparisonByName);
        //    //    //    Changed();
        //    //    //    break;

        //    //    //case SortingMode.ByAccess:
        //    //    //    FieldList.Sort(MemberComparisonByAccess);
        //    //    //    OperationList.Sort(MemberComparisonByAccess);
        //    //    //    Changed();
        //    //    //    break;

        //    //    //case SortingMode.ByKind:
        //    //    //    FieldList.Sort(MemberComparisonByKind);
        //    //    //    OperationList.Sort(MemberComparisonByKind);
        //    //    //    Changed();
        //    //    //    break;
        //    //}
        //}
        public bool MoveUpItem(object item)
        {
            //if (item is Field)
            //{
            //    if (MoveUp(FieldList, item))
            //    {
            //        Changed();
            //        return true;
            //    }
            //}
            //else if (item is Operation)
            //{
            //    if (MoveUp(OperationList, item))
            //    {
            //        Changed();
            //        return true;
            //    }
            //}
            return false;
        }

        public bool MoveDownItem(object item)
        {
            //if (item is Field)
            //{
            //    if (MoveDown(FieldList, item))
            //    {
            //        Changed();
            //        return true;
            //    }
            //}
            //else if (item is Operation)
            //{
            //    if (MoveDown(OperationList, item))
            //    {
            //        Changed();
            //        return true;
            //    }
            //}
            return false;
        }
        public abstract Member GetMember(MemberType type, int idx);
        public abstract void AddMember(MemberType type,out Member m);

    }
}
