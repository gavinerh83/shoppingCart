using System;
using System.Collections.Generic;
using CA_Project.Models;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Data
{
    public class SessionDictionary
    {
        private Dictionary<string, long> sessionDict;
        private int Count;

        public SessionDictionary()
        {
            sessionDict = new Dictionary<string, long>();
            Count = 1;
        }

        public void InputStoredSessions(List<Session> s)
        {
            if(Count == 1)
            {
                foreach(Session sess in s)
                {
                    //add only when session time is not expired
                    if((sess.TimeStamp + Session.timeout) > DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        sessionDict.Add(sess.Id.ToString(), sess.TimeStamp);
                    }
                }
                Count++;
            }
        }

        public long? CheckSessionPresence(string sessionid)
        {
            if (sessionDict.ContainsKey(sessionid))
            {
                return sessionDict[sessionid];
            }
            return null;
        }

        public bool AddSession(string sessionid, long timestamp)
        {
            if (!sessionDict.ContainsKey(sessionid))
            {
                sessionDict[sessionid] = timestamp;
                return true;
            }
            return false;
        }


        public bool RemoveSession(string sessionid)
        {
            if (sessionDict.ContainsKey(sessionid))
            {
                sessionDict.Remove(sessionid);
                return true;
            }
            return false;
        }


    }
}
