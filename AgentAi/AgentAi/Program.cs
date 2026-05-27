// See https://aka.ms/new-console-template for more information
using System.Text;
using GenerativeAI.Microsoft;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

Console.WriteLine("Hello, World!");


IChatClient client = new GenerativeAIChatClient("<Your API Key>", "<Modle>");
//ChatClientAgent agent = new(client);

//AgentResponse response = await agent.RunAsync("How to make sope");

//Console.WriteLine(response);
await using McpClient mcpClient = await McpClient.CreateAsync(new HttpClientTransport(new HttpClientTransportOptions
{
    Endpoint = new Uri("https://learn.microsoft.com/api/mcp"),
    TransportMode = HttpTransportMode.StreamableHttp
}));
IList<McpClientTool> mcpTools = await mcpClient.ListToolsAsync();

//Create Agent
ChatClientAgent agent = client
    .AsAIAgent(
        instructions: "You are an Expert in the C# version of Microsoft Agent Framework " +
                      "(use tools to find your knowledge) " +
                      "and assume Azure OpenAI with API Key is used"
        ,tools: mcpTools.Cast<AITool>().ToList()
    );

AgentSession session = await agent.CreateSessionAsync();

Console.OutputEncoding = Encoding.UTF8;
while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine() ?? "";
    AgentResponse response = await agent.RunAsync(input, session);
    {
        Console.WriteLine(response);
    }

    Output.Separator();
}





public static class Output
{
    public static void Red(string message)
    {
        WriteLine(message, ConsoleColor.Red);
    }

    public static void Green(string message)
    {
        WriteLine(message, ConsoleColor.Green);
    }

    public static void Yellow(string message)
    {
        WriteLine(message, ConsoleColor.Yellow);
    }

    public static void Gray(string message)
    {
        WriteLine(message, ConsoleColor.DarkGray);
    }

    public static void Blue(string message)
    {
        WriteLine(message, ConsoleColor.Blue);
    }

    public static void Magenta(string message)
    {
        WriteLine(message, ConsoleColor.DarkMagenta);
    }

    public static void Separator(bool preAndPostLinebreak = true)
    {
        if (preAndPostLinebreak)
        {
            Console.WriteLine();
        }

        WriteLine("".PadLeft(Console.WindowWidth, '-'), ConsoleColor.Gray);

        if (preAndPostLinebreak)
        {
            Console.WriteLine();
        }
    }

    private static void WriteLine(string text, ConsoleColor color)
    {
        ConsoleColor orgColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
        finally
        {
            Console.ForegroundColor = orgColor;
        }
    }

    public static void Title(string title)
    {
        Green(title);
    }
}