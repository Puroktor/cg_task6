namespace cg_task6;
struct Triangle
{
    public Point P1 { get; set; }

    public Point P2 { get; set; }

    public Point P3 { get; set; }

    public Point this[int i]
    {
        get
        {
            return i switch
            {
                0 => P1,
                1 => P2,
                2 => P3,
                _ => throw new IndexOutOfRangeException(),
            };
        }
        set
        {
            switch (i)
            {
                case 0: P1 = value; break;
                case 1: P2 = value; break;
                case 2: P3 = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }
    }
}

