using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PianoLab;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int FirstMidiNote = 21;
    private const int LastMidiNote = 108;
    private const double WhiteKeyWidth = 24;

    private static readonly string[] NoteNames =
    [
        "C", "Cs", "D", "Ds", "E", "F", "Fs", "G", "Gs", "A", "As", "B"
    ];

    public MainWindow()
    {
        InitializeComponent();
        Generate_Keyboard();
    }

    private void PianoKey_Click(object sender, MouseButtonEventArgs e)
    {
        Border key = (Border)sender;

        string note = key.Tag?.ToString() ?? "unknown";

        MessageBox.Show($"You played {note}");
    }

    private void Generate_Keyboard()
    {
        PianoCanvas.Children.Clear();

        int whiteKeyIndex = 0;

        for (int midiNote = FirstMidiNote; midiNote <= LastMidiNote; midiNote++)
        {
            if (!IsBlackKey(midiNote))
            {
                PianoCanvas.Children.Add(CreateKey(midiNote, false, whiteKeyIndex));
                whiteKeyIndex++;
            }
        }

        whiteKeyIndex = 0;

        for (int midiNote = FirstMidiNote; midiNote <= LastMidiNote; midiNote++)
        {
            if (IsBlackKey(midiNote))
            {
                PianoCanvas.Children.Add(CreateKey(midiNote, true, whiteKeyIndex));
            }
            else
            {
                whiteKeyIndex++;
            }
        }

        PianoCanvas.Width = whiteKeyIndex * WhiteKeyWidth;
    }

    private Border CreateKey(int midiNote, bool isBlackKey, int whiteKeyIndex)
    {
        int octave = midiNote / 12 - 1;
        string noteName = $"Key_{NoteNames[midiNote % 12]}{octave}";

        Border key = new()
        {
            Name = noteName,
            Tag = midiNote,
            Width = isBlackKey ? 14 : WhiteKeyWidth,
            Height = isBlackKey ? 95 : 150,
            Style = (Style)FindResource(isBlackKey ? "BlackKeyStyle" : "WhiteKeyStyle")
        };

        Canvas.SetLeft(key, isBlackKey ? whiteKeyIndex * WhiteKeyWidth - 7 : whiteKeyIndex * WhiteKeyWidth);
        Canvas.SetTop(key, 0);
        Panel.SetZIndex(key, isBlackKey ? 1 : 0);
        key.MouseLeftButtonDown += PianoKey_Click;
        return key;
    }

    private static bool IsBlackKey(int midiNote)
    {
        return midiNote % 12 is 1 or 3 or 6 or 8 or 10;
    }
}