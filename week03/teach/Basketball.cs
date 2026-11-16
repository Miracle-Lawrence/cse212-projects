/*
 * CSE 212 Lesson 6C
 *
 * This code analyzes NBA basketball data and displays
 * the players with the top 10 career points.
 */

using Microsoft.VisualBasic.FileIO;

public class Basketball
{
    public static void Run()
    {
        var players = new Dictionary<string, int>();

        using var reader = new TextFieldParser("basketball.csv");
        reader.TextFieldType = FieldType.Delimited;
        reader.SetDelimiters(",");
        reader.ReadFields(); // Ignore header row

        while (!reader.EndOfData)
        {
            var fields = reader.ReadFields()!;
            var playerId = fields[0];
            var points = int.Parse(fields[8]);

            if (players.ContainsKey(playerId))
                players[playerId] += points;
            else
                players[playerId] = points;
        }

        var topPlayers = players.ToArray();
        Array.Sort(topPlayers, (p1, p2) => p2.Value - p1.Value);

        Console.WriteLine();

        for (var i = 0; i < 10; ++i)
        {
            Console.WriteLine(topPlayers[i]);
        }
    }
}
