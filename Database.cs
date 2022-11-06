using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Lab6Starter;
/**
 * Name: Shabbar & Thomas
 * Date: 11/05/2022
 * Description:This the main page for the Tic-Tac-Toe application
 * Bugs: n/a
 * Reflection: It was fairly simple, but a valueable experience in 
 * forking repos, GitHub, and paired programming.
 */
internal class Database {
    String connectionString;
    ObservableCollection<ResultEntry> entries
        = new ObservableCollection<ResultEntry>();

    /// <summary>
    /// Here or thereabouts initialize a connectionString that will be used in all the SQL calls
    /// </summary>
    public Database() {
        connectionString = InitializeConnectionString();
        GetEntries();
    }

    /// <summary> Adds the result of a game as an entry to the bit.io db
    /// </summary>
    /// <param name="entry">the ResultEntry of the game</param>
    public void AddResultEntry( ResultEntry entry ) {
        try {
            // this will find an id that isn't used already
            foreach ( ResultEntry curr in entries ) {
                while ( curr.Id == entry.Id ) {
                    entry.Id++;
                }
            }
            entries.Add( entry );

            NpgsqlConnection con = new NpgsqlConnection( connectionString );
            con.Open();
            var sql = "INSERT INTO results " +
                "(id, date, winner, time) " +
                "VALUES (@id, @date, @winner, @time)";

            using var cmd = new NpgsqlCommand( sql, con ) {
                Parameters = {
                    new( "id", entry.Id ),
                    new( "date", entry.Date ),
                    new( "winner", entry.Winner ),
                    new( "time", entry.Time )
                }
            };
            using NpgsqlDataReader reader = cmd.ExecuteReader();
            con.Close();
        } catch ( IOException ioe ) {
            Console.WriteLine( "Error while adding entry: {0}", ioe );
        }
    }

    /// <summary> Returns a list of the entries
    /// </summary>
    /// <returns>a list of the entries</returns>
    public ObservableCollection<ResultEntry> GetEntries() {
        if (entries.Count > 0) {
            return entries;
        }
        try {
            NpgsqlConnection con = new NpgsqlConnection( connectionString );
            con.Open();
            var sql = "SELECT * FROM \"results\" limit 10;";
            using var cmd = new NpgsqlCommand( sql, con );
            using NpgsqlDataReader reader = cmd.ExecuteReader();

            // Columns are clue, answer, difficulty, date, id in that order ...
            // Show all data
            while ( reader.Read() ) {
                for ( int colNum = 0; colNum < reader.FieldCount; colNum++ ) {
                    Console.Write( reader.GetName( colNum ) + "=" + reader[ colNum ] + " " );
                }
                Console.Write( "\n" );
                entries.Add( new ResultEntry(
                    (int) reader[ 0 ],
                    reader[ 1 ] as String,
                    reader[ 2 ] as String,
                    reader[ 3 ] as String )
                    );
            }
            con.Close();
        } catch ( IOException ioe ) {
            Console.WriteLine( "Error while retrieving entries: {0}", ioe );
        }
        return entries;
    }

    /// <summary>
    /// Creates the connection string to be utilized throughout the program
    /// </summary>
    public String InitializeConnectionString() {
        var bitHost = "db.bit.io";
        var bitApiKey = "v2_3vMDr_UenbUjpMy9Px33bRRz3fbVn"; // from the "Password" field of the "Connect" menu
        var bitUser = "pelegt46";
        var bitDbName = "pelegt46/Tic-Tac-Toe";

        return connectionString = $"Host={bitHost};Username={bitUser};" +
            $"Password={bitApiKey};Database={bitDbName};";
    }
}