using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MappingBreakDown
{
    class Interval
    {
        private string name;
        private int start;
        private int end;

        public Interval(string name, int start, int end)
        {
            this.name = name;
            this.start = start;
            this.end = end;
        }

        public int getStart()
        {
            return start;
        }

        public int getEnd()
        {
            return end;
        }
        
        public string getName()
        {
            return name;
        }

        private bool IsInInterval(int num)
        {
            return num >= start && num <= end;
        }

        public bool IsIntersect(Interval other)
        {
            int start_other = other.getStart();
            int end_other = other.getEnd();
            for (int i = start_other; i <= end_other; i++)
                if (IsInInterval(i))
                    return true;
            return false;
        }

        public static Tuple<string, string> IsIntersectList(List<Interval> lst)
        {
            for (int i = 0; i < lst.Count; i++)
                for (int j = i + 1; j < lst.Count; j++)
                    if (lst[i].IsIntersect(lst[j]))
                        return Tuple.Create(lst[i].name, lst[j].name);
            return Tuple.Create("","");
        }
    }
}
