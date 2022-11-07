//using Android.Service.QuickSettings;
using System.Collections.ObjectModel;

namespace Lab6Starter;
/**
 * Name: Shabbar & Thomas
 * Date: 11/05/2022
 * Description: This the main content page for the Tic-Tac-Toe application
 * Bugs: n/a
 * Reflection: It was okay. The scoring, celebratory message, and game reset
 * feature was mistakenly done before A forked B's repo. However, it was
 * still a valueable experience for both of us because we got valueable 
 * experience with forking repos, GitHub, and paired programming.
 */
/// <summary>
/// The MainPage, this is a 1-screen app
/// </summary>
public partial class MainPage : ContentPage {
    TicTacToeGame ticTacToe;                // model class
    Button[,] grid;                         // stores the buttons

    private static TimeOnly timer;          // timer object
    private static bool gameOver;
    Database db;

    /// <summary>
    /// initializes the component
    /// </summary>
    public MainPage() {
        InitializeComponent();
        db = new Database();
        EntriesLV.ItemsSource = db.GetEntries();
        ticTacToe = new TicTacToeGame();
        grid = new Button[ TicTacToeGame.GRID_SIZE, TicTacToeGame.GRID_SIZE ] { 
            { Tile00, Tile01, Tile02 }, 
            { Tile10, Tile11, Tile12 }, 
            { Tile20, Tile21, Tile22 } 
        };
        RandomizeTileColors();
        gameOver = false;
        StartGameTimer();
    }

    /// <summary>
    /// Handles button clicks - changes the button to an X or O (depending on whose turn it is)
    /// Checks to see if there is a victory - if so, invoke 
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleButtonClick( object sender, EventArgs e ) {
        Player victor;
        Player currentPlayer = ticTacToe.CurrentPlayer;

        Button button = (Button) sender;
        int row;
        int col;

        FindTappedButtonRowCol( button, out row, out col );
        if ( !button.Text.ToString().Equals( "" ) ) { // if the button has an X or O, bail
            DisplayAlert( "Illegal move", "Fill this in with something more meaningful", "OK" );
        } else {
            button.Text = currentPlayer.ToString();
            gameOver = ticTacToe.ProcessTurn( row, col, out victor );

            if ( gameOver ) {
                CelebrateVictory( victor );
            }
        }
    }

    /// <summary>
    /// Returns the row and col of the clicked row
    /// There used to be an easier way to do this ...
    /// </summary>
    /// <param name="button"></param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private void FindTappedButtonRowCol( Button button, out int row, out int col ) {
        row = -1;
        col = -1;

        for ( int r = 0; r < TicTacToeGame.GRID_SIZE; r++ ) {
            for ( int c = 0; c < TicTacToeGame.GRID_SIZE; c++ ) {
                if ( button == grid[ r, c ] ) {
                    row = r;
                    col = c;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Celebrates victory, displaying a message box and resetting the game
    /// </summary>
    private async void CelebrateVictory( Player victor ) {
        await DisplayAlert( "Victory",
            String.Format( "Congratulations, {0}, you're the big winner today",
                victor.ToString() ), "OK" );

        if ( victor.Equals( "O" ) ) {
            XScoreLBL.Text = String.Format( "X's Score: {0}", ticTacToe.XScore );
            OScoreLBL.Text = String.Format( "O's Score: {0}", ++ticTacToe.OScore );
        } else if ( victor.Equals( "X" ) ) {
            XScoreLBL.Text = String.Format( "X's Score: {0}", ++ticTacToe.XScore );
            OScoreLBL.Text = String.Format( "O's Score: {0}", ticTacToe.OScore );
        } else {
            XScoreLBL.Text = String.Format( "X's Score: {0}", ++ticTacToe.XScore );
            OScoreLBL.Text = String.Format( "O's Score: {0}", ticTacToe.OScore );
        }
        AddResultEntry(
            db.GetEntries().Count + 1,
            DateOnly.FromDateTime( DateTime.Now ).ToString(),
            victor.ToString(),
            TimeLabel.Text );
        ResetGame();
    }

    /// <summary> This is the game timer for the tic-tac-toe 
    /// </summary>
    public async void StartGameTimer() {
        timer = new();

        while ( !gameOver ) {
            await Task.Delay( TimeSpan.FromSeconds( 1 ) );
            timer = timer.Add( TimeSpan.FromSeconds( 1 ) );
            TimeLabel.Text = $"{timer.Minute}m {timer.Second: 0}s";
        }
    }

    /// <summary> Randomly colors the tiles of the tic-tac-toe board
    /// </summary>
    public void RandomizeTileColors() {
        Random rng = new();
        int red;
        int green;
        int blue;
        int alpha;

        foreach ( Button tile in grid ) {
            red = rng.Next( 255 );
            green = rng.Next( 255 );
            blue = rng.Next( 255 );
            // alpha is offset so that way the colors appear lighter and it's more visibl
            alpha = rng.Next( 128 ) + 64;       
            tile.BackgroundColor = Color.FromRgba( red, green, blue, alpha );
        }
    }

    /// <summary>
    /// Resets the grid buttons so their content is all ""
    /// </summary>
    private void ResetGame() {
        foreach (Button tile in grid) {
            tile.Text = "";
        }
        RandomizeTileColors();
        ticTacToe.ResetGame();
        gameOver = false;
        StartGameTimer();
    }


    /// <summary> Adds a new tic-tac-toe game result to the bit.io database
    /// </summary>
    public void AddResultEntry(int id, String date, String winner, String time) {
        db.AddResultEntry( new ResultEntry( id, date, winner, time ) );
    }
}