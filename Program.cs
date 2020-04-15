using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadAndTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //提示，先换行，再输入实际要计算的值
            CommonClass.ShowTip();
            while (Console.ReadLine() != null)
            {
                int number = 0;
                string readValue = Console.ReadLine();

                //thread另开两个线程计算结果
                ThreadTest threadTest = new ThreadTest();
                threadTest.ThreadFunction(readValue);

                //task另开线程计算结果
                TaskTest taskTest = new TaskTest();                
                int.TryParse(readValue, out number);
                taskTest.TaskFunction(number);

                ////主线程计算结果
                Console.WriteLine("FOO Result:" + CommonClass.Foo(number));
                Console.WriteLine("Add Result:" + CommonClass.Add(number));

                //提示，先换行，再输入实际要计算的值
                CommonClass.ShowTip();

            }
        }
    }

    public class ThreadTest
    {
        private delegate void delegateFoo(object number);
        private delegate void delegateAdd(object number);
        delegateFoo foo;
        delegateAdd add;
        public void ThreadFunction(object obj)
        {
            foo = CommonClass.FooForDelegate;
            add = CommonClass.AddForDelegate;
            Thread threadFoo = new Thread(new ParameterizedThreadStart(foo));            
            Thread threadAdd = new Thread(new ParameterizedThreadStart(add));

            threadFoo.Start(obj);
            threadAdd.Start(obj);
        }
    }

    public class TaskTest
    {
        public void TaskFunction(int number)
        {
            //TaskFactory tf = new TaskFactory();
            //Task task1 = tf.StartNew(NewTask);
            //Task task2 = Task.Factory.StartNew(NewTask);
            //Task task3 = new Task(NewTask);
            //Task task4 = new Task(NewTask,TaskCreationOptions.PreferFairness);
            //task3.Start();
            //task4.Start();

            Task<int> taskFoo = new Task<int>(() => CallFoo(number));
            Task<int> taskAdd = new Task<int>(() => CallAdd(number));
            taskFoo.Start();
            taskAdd.Start();
            Console.WriteLine("Task FOO result:" + taskFoo.Result);
            Console.WriteLine("Task Add result:" + taskAdd.Result);
        }
        /// <summary>
        /// 无参方法
        /// </summary>
        public void NewTask()
        {
            Console.WriteLine("开始一个任务");
            Console.WriteLine("Task id:{0}", Task.CurrentId);
            Console.WriteLine("任务执行完成");
        }

        public int CallFoo(int number)
        {
            return CommonClass.Foo(number);
        }
        public int CallAdd(int number)
        {
            return CommonClass.Add(number);
        }
    }

    public static class CommonClass
    {
        /// <summary>
        /// 递归,求裴波拉切数列值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int Foo(int i)
        {
            //Console.WriteLine("FOO:" + i);
            if (i <= 0)
                return 0;
            else if (i > 0 && i <= 2)
                return 1;
            else return Foo(i - 1) + Foo(i - 2);
        }
        /// <summary>
        /// 通过临时变量，求裴波拉切数列值
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int Add(int i)
        {
            if (i <= 0)
                return 0;
            else if (i > 0 && i <= 2)
                return 1;
            else
            {
                int result = 0;
                int firstTemp = 1;
                int secondTemp = 1;
                for (int j = 1; j <= i; j++)
                {
                    //Console.WriteLine("ADD:"+j);
                    if (j == 1)
                        firstTemp = 1;
                    if (j == 2)
                        secondTemp = 1;
                    else
                    {
                        result = firstTemp + secondTemp;
                        firstTemp = secondTemp;
                        secondTemp = result;
                    }
                }
                return result;
            }
        }

        public static void ShowTip()
        {
            Console.WriteLine("Please input a line first, then input the actually value!");
        }

        public static void FooForDelegate(object number)
        {
            int i = 0;
            int.TryParse(number?.ToString(),out i);
            //Console.WriteLine("FOO begin time:" + DateTime.Now.ToString("HH:mm:ss ssss"));
            DateTime beginTime = DateTime.Now;
            int result = Foo(i);
            DateTime endTime = DateTime.Now;
            //Console.WriteLine("FOO end time:" + DateTime.Now.ToString("HH:mm:ss ssss"));
            Console.WriteLine(string.Format("Thread Input value:{0};convert to int value is:{1};Foo Result:{2};used time:{3}",number,i, result, endTime-beginTime));
        }

        public static void AddForDelegate(object number)
        {
            int i = 0;
            int.TryParse(number?.ToString(), out i);
            //Console.WriteLine("ADD begin time:" + DateTime.Now.ToString("HH:mm:ss ssss"));
            DateTime beginTime = DateTime.Now;
            int result = Add(i);
            DateTime endTime = DateTime.Now;
            //Console.WriteLine("ADD end time:" + DateTime.Now.ToString("HH:mm:ss ssss"));
            Console.WriteLine(string.Format("Thread Input value:{0};convert to int value is:{1};Add Result:{2};used time:{3}", number, i, result, endTime - beginTime));
        }

    }
    
}
