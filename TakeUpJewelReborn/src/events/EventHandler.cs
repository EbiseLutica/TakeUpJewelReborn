namespace TakeUpJewel
{
	public delegate void StaticEventHandler<TEventArgs>(TEventArgs e);
	public delegate void PreEventHandler(object? sender, PreEventArgs e);
}