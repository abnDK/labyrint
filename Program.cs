namespace Labyrint

{

    public class Program
    {
        static void Main()
        {

            // ready for making a dfs algo looking for specific block based on list of paths (getPaths())

            Labyrint l = new Labyrint();

            Console.WriteLine(l.Board.AvailableBlock.Sides.ToString());

            l.Board.renderField();

            Block testBlock = new Block(9, 9, "testBlock");

            Block clonedBlock = (Block)testBlock.Clone();

            l.SearchGoalForce("0,0", "8,8");

            int y = 10;
            int x = 5;
            System.Console.WriteLine($"{y}, {x}");
            x = y;
            System.Console.WriteLine($"{y}, {x}");
            y = 20;
            System.Console.WriteLine($"{y}, {x}");
            System.Console.WriteLine("####");
            MyClass mc = new MyClass(10, 20);
            MyClass clone = mc;
            System.Console.WriteLine($"{mc.N}, {mc.U}\n{clone.N}, {clone.U}");
            mc.N = 30;
            System.Console.WriteLine($"{mc.N}, {mc.U}\n{clone.N}, {clone.U}");
            MyClass clone2 = new MyClass(100, 200);
            clone2.U = 500;
            System.Console.WriteLine($"{mc.N}, {mc.U}\n{clone.N}, {clone.U}");

            MyClass ShallowClone = (MyClass)mc.CloneShallow();
            MyClass DeepClone = (MyClass)mc.CloneDeep();

            System.Console.WriteLine($"Orig: {mc.N}\nShallow: {ShallowClone.N}\nDeep: {DeepClone.N}\n");
            mc.N = 1;
            System.Console.WriteLine($"Orig: {mc.N}\nShallow: {ShallowClone.N}\nDeep: {DeepClone.N}\n");
            ShallowClone.N = 100;
            DeepClone.N = 500;
            System.Console.WriteLine($"Orig: {mc.N}\nShallow: {ShallowClone.N}\nDeep: {DeepClone.N}\n");


        }
    }

    public class MyClass
    {


        public int N { get; set; }
        public int U { get; set; }
        public MyClass(int n, int u)
        {
            this.N = n;
            U = u;
        }
        public object CloneShallow()
        {
            return this;
        }

        public object CloneDeep()
        {
            MyClass clone = new MyClass(0, 0);
            clone.N = this.N;
            clone.U = this.U;
            return clone;
        }
    }

}
