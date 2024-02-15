namespace AlgoApp.AsyncLessons;

public class TaskWhenAllExamples
{
   
    public static async Task CheckWhenTaskThrowsError()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(12000);
        var tasks = new List<Task> ();

        for(int i = 2; i < 15 ; i+=2 )
        {
            tasks.Add(Wait(i, cancellationTokenSource.Token));
        }
        var t = Task.WhenAll(tasks);
        try
        {
            await t;
            //await Task.WhenAll(tasks);
            Console.WriteLine("done");
        }
        catch(AggregateException ex)
        {
           var y = tasks.Where(x => x.IsCanceled);
            Console.WriteLine(ex.Message);
        }
         catch(Exception ex)
        {
           var y = tasks.Where(x => x.IsCanceled);
            Console.WriteLine(ex.Message);
        }
        
    }

    public static async Task Wait(int sec, CancellationToken token)
    {
        await Task.Delay(sec * 1000,token);
        if(sec % 3 == 0) throw new Exception ($"you dey whine me ni? {sec}" );
    }
}


