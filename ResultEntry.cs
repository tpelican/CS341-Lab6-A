using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Lab6Starter;
/**
 * Name: Shabbar & Thomas
 * Date: 11/05/2022
 * Description: Represents an result entry for the game
 * Bugs: n/a
 * Reflection: It was fairly simple, but a valueable experience in 
 * forking repos, GitHub, and paired programming.
 */
internal class ResultEntry : ObservableObject {
    private int id;
    private String date;
    private String winner;
    private String time;
    public int Id {
        get { return id; }
        set { SetProperty( ref id, value ); }
    }
    public String Date {
        get { return date; }
        set { SetProperty( ref date, value ); }
    }

    public String Winner {
        get { return winner; }
        set { SetProperty( ref winner, value ); }
    }

    public String Time {
        get { return time; }
        set { SetProperty( ref time, value ); }
    }

    /// <summary> Creates a new result entry
    /// </summary>
    /// <param name="id"> the id of the entry </param>
    /// <param name="date"> the current date</param>
    /// <param name="winner"> the winner of the game</param>
    /// <param name="time"> time amount of time the game took</param>
    public ResultEntry(int id, string date, string winner, string time) {
        this.id = id;
        this.date = date;
        this.winner = winner;
        this.time = time;
    }
}