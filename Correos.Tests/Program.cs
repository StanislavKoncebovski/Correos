using Correos;

[Correos]
class MyTestClass
{
	[RequestTarget]
	public void MyRequest()
	{

	}
}

static class Program
{
	static void Main()
	{
		Correos.Correos.Register(typeof(MyTestClass));
	}
}
