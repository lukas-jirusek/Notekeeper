using System;							// Console
using System.Collections.Generic;		// List, Dictionary, Tuple
using System.IO;						// files
using System.Text.RegularExpressions;	// Regex.Escape, Regex.Unescape
using System.Linq;						// .Sum() method

namespace Program{
	class Program{
		static void PrintMenu(int numOfNotes){
			/*Prints menu with options*/
			Console.WriteLine("\n\nMenu:");
			if(numOfNotes > 0){
				Console.WriteLine("Cislo <1 - {0}> pro zobrazeni cele poznamky.", numOfNotes);
			}
			Console.WriteLine("'P' pro pridani nove poznamky.");
			if(numOfNotes > 0){
				Console.WriteLine("'S' pro smazani poznamky.");
				Console.WriteLine("'N' pro vypsani nejcasteji pouzivanych slov.");
				Console.WriteLine("'U' pro ulozeni poznamek do souboru.");
			}				
			Console.WriteLine("'L' pro nacteni poznamek z CSV souboru.");
			Console.WriteLine("'X' pro nacteni nekolika vzorovych poznamek (pouze pro ukazku prace programu).");
			Console.WriteLine("'K' pro ukonceni programu.");
			Notekeeper.HelperFunctions.PrintLine();
		}
		static bool PerformUserAction(List<Notekeeper.Note> notes, string action){
			/*Parform some action given user input*/
			int index;
			switch(action.ToLower()){
				case "p":
					/*Option to create new node*/
					Notekeeper.Note newNote = new Notekeeper.Note();
					newNote.createNote();
					notes.Add(newNote);
					/*add it to the list*/
					notes.Sort((x, y) => x.GetCreationTime().CompareTo(y.GetCreationTime()));
					/*sort it just in case there is somehow newer note that the one thats just created*/
					Console.WriteLine("Poznamka pridana, pokracujte zmacknutim klavesy.");
					Console.ReadKey();
					break;

				case "s":
					/* Option to remove note (if there is any), get index of the note to remove*/
					if(notes.Count == 0){
						Console.WriteLine("Jeste jste nepridali pozmanku. pokracujte stisknutim klavesy.");
						Console.ReadKey();
						break;
					}
					while((index = Notekeeper.HelperFunctions.GetIndex(notes.Count)) == -1){
						Console.WriteLine("Neplatne cislo poznamky. Zkuste to znovu.");
					}
					notes.RemoveAt(index);
					Console.WriteLine("Poznamka odstranena. pokracujte stisknutim klavesy.");
					Console.ReadKey();
					break;

				case "n":
					/*Option to print notes stats, do that if there are any.*/
					if(notes.Count == 0){
						Console.WriteLine("Zatim nemate zadne poznamky, pokracujte stisknutim klavesy.");
						Console.ReadKey();
						break;
					}
					Console.Clear();
					Notekeeper.HelperFunctions.PrintStats(notes);
					break;

				case "k":
					/*Option to end the program*/
					if(notes.Count == 0){
						/*If there are no notes, just leave*/
						Console.WriteLine("Mej hezky den!");
						return true;
					}
					Console.Write("Jsi si jist, neulozena prace bude ztracena, ukonceni potvrd klavesou 'Y' nebo <Enter> > ");
					string response = Console.ReadLine().ToLower();
					if(response == "" || response[0] == 'y'){
						/*quit on entr or any variation of "yes", should not crash thanks to short circuit evaluation*/
						Console.WriteLine("Mej hezky den!");
						return true;
					}
					break;

				case "u":
					/*Save notes to CSV file.*/
					Notekeeper.HelperFunctions.SaveToCSV(notes);
					break;

				case "l":
					/*Load notes from CSV, re-sort so that notes are from oldest to newest.*/
					Notekeeper.HelperFunctions.LoadFromCSV(notes);
					notes.Sort((x, y) => x.GetCreationTime().CompareTo(y.GetCreationTime()));
					break;

				case "x":
					Notekeeper.Note sample1 = new Notekeeper.Note("Poznamka z budoucnosti!", "Tato poznamka existuje, prestoze by nemela.\nDatum jejich vytvoreni jsem totiz upravil manualne a jelikoz jsou poznamky serazeny podle data, mela by tato poznamka byt jako posledni, pokud tedy nekdo nenajde zpusob jak vytvorit poznamku jeste novejsi a to budto pozmenenim kodu nebo upravou dat v .csv souboru");
					sample1.SetCreationTime(new DateTime(2022, 9, 10, 18, 55, 34));
					Notekeeper.Note sample2 = new Notekeeper.Note("Vanocni poznamka!", "Muj mily denicku, dnes jsou Vanoce na ktere jsem se tak dlouho tesil!\nK veceri budou rizky a dostanu hodne darku, uz se nemuzu dockat.");
					sample2.SetCreationTime(new DateTime(2019, 12, 24, 12, 45, 0));
					Notekeeper.Note sample3 = new Notekeeper.Note("", "Tato poznamka nema zadny titulek, pro lepsi citelnost je ale v nahledu zobrazeno ze titulek skutecne zadny neni.");
					sample3.SetCreationTime(new DateTime(2020, 5, 4, 14, 22, 36));
					Notekeeper.Note sample4 = new Notekeeper.Note("Kratka poznamka se v nahledu zobrazi cela", "Kratka poznamka");
					sample4.SetCreationTime(new DateTime(2020, 5, 5, 13, 4, 21));
					Notekeeper.Note sample5 = new Notekeeper.Note("Dlouha poznamka", "Tady se opravdu rozkecam aby byla tahle poznamka hezky dlouha,\nbudu se take snazit radkovat\n\npro jistotu i vicekrat.\n\n\nDlouhou poznamku chci proto, aby bylo hodne slov ktera se pak spocitaji a ukazi pokud mozno co hodne dat ve statistikach poznamek. Bohuzel jsem uz dlouho neodradkoval a jelikoz konzole nema problem s rozpulenim slova, bude text teto poznamky dost pravdepodobne ne prilis sikovne rozdelen.\nTake se priznam ze vymyslet tento vzorovy text neni zadna sranda\na ze uz skutecne nevim co vic do teto poznamky napsat.\n\nProto ji asi zde ukoncim.");
					sample5.SetCreationTime(new DateTime(2020, 5, 4, 14, 22, 36));
					Notekeeper.Note sample6 = new Notekeeper.Note("Stara poznamka", "Tato poznamka je naopak velmi stara a proto by se mela zobrazit jako prvni.");
					sample6.SetCreationTime(new DateTime(1803, 1, 8, 19, 14, 30));
					Notekeeper.HelperFunctions.AddNote(notes, sample1);
					Notekeeper.HelperFunctions.AddNote(notes, sample2);
					Notekeeper.HelperFunctions.AddNote(notes, sample3);
					Notekeeper.HelperFunctions.AddNote(notes, sample4);
					Notekeeper.HelperFunctions.AddNote(notes, sample5);
					Notekeeper.HelperFunctions.AddNote(notes, sample6);

					notes.Sort((x, y) => x.GetCreationTime().CompareTo(y.GetCreationTime()));
					Console.WriteLine("Vzorove poznamky pridany, pokracujte stisknutim klavesy.");
					Console.ReadKey();
					break;

				case "":
					/*No entry = do nothing*/
					break;

				default:
					/*No single letter selection - user either inputted number of note to view or invalid input.*/
					index = Notekeeper.HelperFunctions.GetIndex(notes.Count, action);
					/*Try to get index from users response*/
					if(index == -1){
						/*If input was invalid, do nothing.*/
						Console.WriteLine("Neplatna moznost nebo cislo poznamky: '{0}', pokracujte stisknutim klavesy.", action);
						Console.ReadKey();
						break;
					}
					/*Valid input = show note.*/
					notes[index].showNote();
					break;
			}
			return false;
		}
		static int Main(){
			List<Notekeeper.Note> notes = new List<Notekeeper.Note>();
			/*master list keeping all notes together*/

			string response;
			/*string for user response*/

			Console.Clear();
			Console.WriteLine("Vitejte u vasich poznamek!");
			while(true){
				/*main loop*/
				Notekeeper.HelperFunctions.PrintPreview(notes);
				/* show just the previes*/
				PrintMenu(notes.Count);
				/*print options*/

				Console.Write("Vase volba: > ");
				/*get response ..*/
				response = Console.ReadLine();
				if(PerformUserAction(notes, response)){
					break;
				}
				/*and act based on it, if true is returned, user chose to quit*/

				Console.Clear();
			}
			return 0;
		}
	}
}

namespace Notekeeper{
	using CounterDict = Dictionary<string, uint>;		/* Dictionary counting occurances */
	using SortedCounterEntry = Tuple<string, uint>;		/* a pair of a word and its occurance */
	using SortedCounter = List<Tuple<string, uint>>;	/* list of those pairs */
	/* some new types for easier manipulation of data */

	static class HelperFunctions{
		/*This class contains only static methods used in somewhere else.*/
		static public string CleanWord(string word){
			/* Returns lowercase vaerion of given word with non-alphabetic characters removed.*/
			string result = "";

			foreach (char character in word)
				if (char.IsLetter(character))
					result += char.ToLower(character);
			return result;
		}
		static public SortedCounter sortDictionary(CounterDict counter){
			/*
			Sorts dictionary and returns it's data as SortedCounter aka List<Tuple<string, uint>>
			where Tuple<string, uint> is a pair of a word and its occurance.
			*/
			SortedCounter result = new SortedCounter();
			foreach (string key in counter.Keys){
				result.Add(new SortedCounterEntry(key, counter[key]));
				/* Add each pair or word - occurance*/
			}
			result.Sort((x, y) => y.Item2.CompareTo(x.Item2));
			/*Sort them in descending order by occurance.*/
			return result;
		}
		static public CounterDict addWordCounters(List<Note> notes){
			/*Adds CounterDicts from all notes into one CounterDict, which is returned*/
			CounterDict counter = new CounterDict();
			CounterDict noteStats;

			foreach(Note note in notes){
				noteStats = note.GetWordCount();
				foreach(string key in noteStats.Keys){
					/*for each key in each note*/
					if(!counter.ContainsKey(key))
						/*is key does not exist yet, add it*/
						counter[key] = 0;
					counter[key] += noteStats[key];
					/*and add the count*/
				}
			}
			return counter;
		}
		static public void PrintStats(List<Note> notes){
			/*Prints statistics about notes - total number of words and topN most used words*/
			CounterDict counter;
			SortedCounter sortedCounter;
			uint totalWords;
			const int topN = 10;
			
			counter = addWordCounters(notes);
			totalWords = (uint)(counter.Sum(x => x.Value));
			sortedCounter = sortDictionary(counter).GetRange(0, topN);
			/*Add counters, sum the values, then sort it and get top N words.*/


			/*And print the information to user, while aligning strings to nice table*/
			Console.WriteLine("Celkovy pocet slov: {0}.", totalWords);
			Console.WriteLine("{0} nejcasteji se objevujicich slov:", topN);
			HelperFunctions.PrintLine();
			Console.WriteLine("{0} | {1}", AlignString("Slovo", 30), AlignString("Pocet pouziti".ToString(), 20));
			HelperFunctions.PrintLine();
			foreach(SortedCounterEntry pair in sortedCounter){
				Console.WriteLine("{0} | {1}", AlignString(pair.Item1, 30), AlignString(pair.Item2.ToString(), 20));
			}
			HelperFunctions.PrintLine();
			Console.WriteLine("Pokracuj stiskem klavesy.");
			Console.ReadKey();
		}
		static public void PrintLine(){
			/*Prints line across whole terminal.*/
			Console.WriteLine(new string('─', Console.WindowWidth - 1));
		}
		static public void PrintPreview(List<Note> notes){
			/* Prints notes to terminal so that they fit into table with columns, shortens strings if required.*/
			string position, title, time, body;		// string used for formatting

			if(notes.Count == 0){
				/*No notes*/
				Console.WriteLine("Jeste nemate zadnou poznamku!");
			}
			else{
				/*Header*/
				Console.WriteLine("Zde jsou nahled vasich poznamek:");
				PrintLine();
				Console.WriteLine("{0} | {1} | {2} | {3}", AlignString("#", 4), AlignString("Titulek", 30), AlignString("Vytvoreno", 20), AlignString("Poznamka", 40));
				PrintLine();
			
				/*Individual notes*/
				for(int i = 0; i < notes.Count; ++i){
					/*format attributes and print them to console*/
					position = AlignString((i+1).ToString() + '.', 4);
					title = AlignString(notes[i].GetTitle(), 30);
					time = AlignString(notes[i].GetCreationTime().ToString(), 20);
					body = AlignString(notes[i].GetBody().Replace("\n", ""), 40);
					Console.WriteLine("{0} | {1} | {2} | {3}", position, title, time, body);
				}
			}
		}
		static public int GetIndex(int max, string input = ""){
			/*
			Returns index between 0 and max-1 either with provided string or with user response.
			Returns -1 whever user inputs number outside this range or invalid string.
			*/
			int index;

			if(input == ""){
				Console.Write("Zadejte cislo poznamky: ");
				input = Console.ReadLine();
			}
			if(int.TryParse(input, out index)){
				if((index >= 1) && (index <= max)){
					return index - 1;
					/*We used position as input - position 1 = index 0, subtract -1 to compensate for this*/
				}
			}
			return -1;
			/*failure*/
		}
		static public void SaveToCSV(List<Note> notes){
			/*Saves list of notes to CSV file. Asks user for filename, verifies it doesnt exist yet.*/
			string filename, result;
			StreamWriter csvFile;

			Console.Write("Zadej nazev CSV souboru kam chce poznamky ulozit: ");
			filename = Console.ReadLine();
			if(filename.Length == 0){
				Console.WriteLine("Nezadal jsi nazev souboru, operace zrusena, pokracuj stisknutim klavesy.");
				Console.ReadKey();
				return;
			}
			if(!filename.EndsWith(".csv")){
				/*Add .csv extension in case user did not put it there.*/
				filename += ".csv";
			}
			if(File.Exists(filename)){
				Console.WriteLine("Soubor '{0}' jiz existuje. Operace zrusena, pokracujte stisknutim klavesy.", filename);
				Console.ReadKey();
				return;
			}
			
			try{
				csvFile = new StreamWriter(filename);
				/*Open file for writing.*/
				foreach(Note note in notes){
					/*Get attributes of each note, separate them with ';'*/
					result = note.GetTitle() + ";" + note.GetCreationTime().ToString() + ";" + note.GetBody().Replace(";", @"\;");
					csvFile.Write(Regex.Escape(result) + "\n");
					/*We must escape string as body of the note can have newline character in it, then add newline character to mark new note entry in csv file*/
				}
				csvFile.Close();
			}
			catch (System.Exception){
				Console.WriteLine("Chyba behem psani souboru, ukoncuji operaci, pokracujte stisknutim klavesy.");
				Console.ReadKey();
				return;
			}
			Console.WriteLine("Poznamky ulozeny do souboru '{0}'. Pokracuj stisknutim klavesy.", filename);
			Console.ReadKey();
		}
		static public void LoadFromCSV(List<Note> notes){
			/*
			Asks for filename, checks its existance and adds notes from the file to list, if the file is not already inside the list.
			Return value: void.
			*/
			int newNotesCounter = 0;
			int duplicateNotesCounter = 0;
			string filename, line;
			StreamReader csvFile;
			Note newNote;
			bool addNote;
			string [] splitLine;


			Console.Write("Zadej nazev CSV souboru s poznamkami: ");
			filename = Console.ReadLine();
			if(filename.Length == 0){
				Console.WriteLine("Nezadal jsi nazev souboru, ukoncuji operaci, pokracuj stiskem klavesy.");
				Console.ReadKey();
				return;
			}
			if(!filename.EndsWith(".csv")){
				/*Add .csv extension if not already in filename.*/
				filename += ".csv";
			}
			if(!File.Exists(filename)){
				/*File does not exist.*/
				Console.WriteLine("Soubor '{0} neexistuje', ukoncuji operaci, pokracuj stiskem klavesy.", filename);
				Console.ReadKey();
				return;
			}
			try{
				csvFile = new StreamReader(filename);
				/*Open reader*/
				while((line = csvFile.ReadLine()) != null){
					/*Read lines as long as there are any*/
					line = Regex.Unescape(line);
					/*unescape previously escaped string.*/
					splitLine = line.Split(';');
					if(splitLine.Length == 3){
						/*Split it and make sure there are 3 fields*/
						newNote = new Note(splitLine[0], splitLine[2]);
						newNote.SetCreationTime(DateTime.Parse(splitLine[1]));
						/*Set note attributes*/

						/*Check if it is duplicate*/
						addNote = true;
						/*first we assume note is unique*/
						foreach(Note note in notes){
							if(note == newNote){
								/*and if we find the note from file matches existing note*/
								addNote = false;
								/*do not add it to list*/
								break;
							}
						}
						if(addNote){
							/*if note is unique*/
							notes.Add(newNote);
							newNotesCounter++;
						}else{
							/*note is duplicite*/
							++duplicateNotesCounter;
						}
					}
					else{
						/*should not happen with files made by this program, as ';' should be automatically removed*/
						Console.WriteLine("Nespravny pocet stredniku na radce: '{0}', ukoncuji operaci.", line);
						break;
					}
				}
			}
			catch (System.Exception){
				Console.WriteLine("Chyba behem cteni souboru, ukoncuji operaci.");
				return;
			}
			Console.WriteLine("{0} novych poznamek nacteno, {1} duplikatnich poznamek preskoceno, pokracujte stisknutim klavesy.", newNotesCounter, duplicateNotesCounter);
			Console.ReadKey();
			return;
		}
		static public string AlignString(string s, int max){
			/*Aligns string to the left, pads up to max characters, if string is longer then max, we take substring and add dots.*/
			if(s.Length < max){
				return s.PadRight(max);
			}
			else{
				return s.Substring(0, max - 3) + "...";
			}
		}
		static public void AddNote(List<Note> notes, Note newNote){
			bool addNote;
			addNote = true;
			foreach(Note note in notes){
				if(note == newNote){
					addNote = false;
					break;
				}
			}
			if(addNote){
				notes.Add(newNote);
			}
		}
	}
	class NoteTextField{
		/*
		Represents generic text field, for example in Note, this class is used both as title and body.
		This class also counts number of occurances of each word, this count is automatically changed whenever content is changed.
		*/
		const string WordSeparators = " \n\t.,?!=-_+*/~;:";
		// all those characters separate words in text
		string text;
		// stores text in given field
		CounterDict wordCount;
		// dictionary maps word (string) and number of occurances of given word (unsigned int)

		public NoteTextField(string text = ""){
			// class constructor, creates Counter dictionary and sets text (default is empty string)
			wordCount = new CounterDict();
			SetText(text);
		}

		public void SetText(string text = ""){
			// sets text of field and counts words 
			this.text = text.Replace(";", "");
			wordCount.Clear();
			// reset counter dictionary
			foreach (string word in text.Split(WordSeparators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)){
				// add each word to counter
				string cleanedWord = HelperFunctions.CleanWord(word);
				if(cleanedWord.Length > 0){
					if (wordCount.ContainsKey(cleanedWord))
						wordCount[cleanedWord] += 1;
					else
						wordCount[cleanedWord] = 1;
				}
			}

		}

		public string GetText() { 
			// returns text in a field as string
			return this.text; 
		}

		public CounterDict GetWordCount() {
			// returns dictionary for counting words
			return this.wordCount;
		}
	}

	class Note{
		/*
		Class representing Note, made of two text fields - title and body, also contains info about note creation.
		*/
		private NoteTextField title;
		private NoteTextField body;
		private DateTime creationTime;
		public Note(string title = "", string body = ""){
			/*Note constructor, assignes provided variables.*/

			this.body = new NoteTextField();
			this.title = new NoteTextField();
			SetTitle(title);
			SetBody(body);
			creationTime = DateTime.Now;
		}
		public void SetCreationTime(DateTime d){
			/*Sets creation time of a Note.*/
			creationTime = d;
		}
		public DateTime GetCreationTime(){
			/*Returns creation time.*/
			return this.creationTime;
		}
		public void SetTitle(string title = ""){
			/*Sets title of Note to given string*/
			this.title.SetText(title);
		}
		public string GetTitle() {
			/*Returns title of Note, returns generic string in case title is empty.*/
			string title = this.title.GetText();
			if(title.Length > 0){
				return title;
			}
			else{
				return "<Bez titulku>";
			}
		}
		public void SetBody(string body = ""){
			/*Sets body of Note to text*/
			this.body.SetText(body);
		}
		public string GetBody() {
			/*Returns body of Note, returns generic string in case body is empty.*/
			string body = this.body.GetText();
			if(body.Length > 0){
				return body;
			}
			else{
				return "<Zadny text poznamky>";
			}
		}
		
		public CounterDict GetWordCount(){
			/*Adds words from title and body into one dictionary.*/
			CounterDict result = new CounterDict(body.GetWordCount());
			CounterDict titleWords = title.GetWordCount();

			foreach (string key in titleWords.Keys){
				if (result.ContainsKey(key)){
					result[key] += titleWords[key];
				}
				else{
					result[key] = titleWords[key];
				}
			}
			return result;
		}
		public void createNote() {
			/* sets attributes of a Node based on user input, user input ends once selected word is written on a line by itself.*/
			string text = "", tmp;

			Console.Clear();
			Console.Write("Zadejte nový titulek poznámky: ");
			this.SetTitle(Console.ReadLine());
			Console.WriteLine("Zadejte text poznamky: (zadavani ukoncite napsanim 'konec' na samostatny radek)");
			HelperFunctions.PrintLine();

			while (true){
				/*Main reading loop*/
				tmp = Console.ReadLine();
				if (tmp.ToLower() == "konec") break;
				/*break if user wants to end note*/
				else text += tmp + '\n';
				/*otherwise append it to text, add a newline as those are stripped by ReadLine()*/
			}
			this.SetBody(text);
		}
		public void showNote() {
			/*Print note to console.*/
			Console.Clear();
			Console.WriteLine("Titulek: {0}\nCas vytvoreni: {1}\nText poznamky:", GetTitle(), creationTime);
			HelperFunctions.PrintLine();
			Console.WriteLine("{0}", GetBody());
			HelperFunctions.PrintLine();
			Console.WriteLine("Pokracuj stisknutim klavesy.");
			Console.ReadKey();
			return;
		}
		public static bool operator==(Note n1, Note n2){
			/*== operator to compare Notes for duplicates.*/
			return ((n1.GetBody() == n2.GetBody()) && (n1.GetCreationTime() == n2.GetCreationTime()) && (n1.GetBody() == n2.GetBody()));
		}
		public static bool operator!=(Note n1, Note n2){
			/*added as C# forces me to define != when == is defined.*/
			return (!((n1.GetBody() == n2.GetBody()) && (n1.GetCreationTime() == n2.GetCreationTime()) && (n1.GetBody() == n2.GetBody())));
		}
	}
}