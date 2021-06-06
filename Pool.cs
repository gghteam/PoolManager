namespace Pool
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Pool<T> : IEnumerable where T : IResettable
    {
        public Pool(IFactory<T> factory) : this(factory, 5) { }

        public Pool(IFactory<T> factory, int poolSize)
        {
            this.factory = factory;
            for(int i = 0; i < poolSize; i++)
            {
                Create();
            }
        }

        ///<summary>members : 풀 안에 생성된 모든 멤버들</summary>        
        public List<T> members = new List<T>();

        ///<summary>unavailable : 이미 사용중인 멤버들</summary>        
        public HashSet<T> unavailable = new HashSet<T>();

        ///<summary>factory : 새 멤버를 생성하기 위한 인터페이스</summary> 
        IFactory<T> factory;

        /// <summary>새 멤버 생성</summary>
        /// <returns>T</returns>
        T Create()
        {
            T member = factory.Create();
            members.Add(member);
            return member;
        }

        /// <summary>멤버를 꺼내거나 혹은 새로 생성.</summary>
        /// <returns>T</returns>        
        public T Allocate()
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (!unavailable.Contains(members[i]))
                {
                    unavailable.Add(members[i]);
                    return members[i];
                }
            }

            T newMembers = Create();
            unavailable.Add(newMembers);
            return newMembers;
        }

        /// <summary>
        /// 멤버를 다시 사용 가능하도록 풀로 돌려놓는다. 멤버에 없을 경우 멤버로 추가.
        /// </summary>
        /// <param name="member">돌려놓을 풀의 멤버</param>
        public void Release(T member)
        {
            member.Reset();
            if (unavailable.Contains(member))
            {
                unavailable.Remove(member);
            }
            else
            {
                members.Add(member);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator<T>)members.GetEnumerator();
        }

        public int GetUsedCount()
        {
            return unavailable.Count;
        }
    }
}

