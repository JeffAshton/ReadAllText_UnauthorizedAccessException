using System;
using System.Collections.Generic;
using System.IO;

namespace FileWatcherReader {

	internal static class Program {

		internal static void Main() {

			string directory = Path.GetFullPath( "." );
			Directory.CreateDirectory( directory );

			string configPath = Path.Combine( directory, "current.txt" );
			string previousPath = Path.Combine( directory, "previous.txt" );

			SortedSet<string> paths = new SortedSet<string>( StringComparer.OrdinalIgnoreCase ) {
				previousPath,
				configPath
			};

			using( FileSystemWatcher fsw = new FileSystemWatcher( directory ) ) {

				fsw.Created += delegate ( object sender, FileSystemEventArgs e ) {
					if( paths.Contains( e.FullPath ) ) {
						ReadFile( paths );
					}
				};
				fsw.Changed += delegate ( object sender, FileSystemEventArgs e ) {
					if( paths.Contains( e.FullPath ) ) {
						ReadFile( paths );
					}
				};
				fsw.Renamed += delegate ( object sender, RenamedEventArgs e ) {
					if( paths.Contains( e.FullPath ) ) {
						ReadFile( paths );
					}
				};

				fsw.EnableRaisingEvents = true;

				Console.ReadLine();
			}
		}

		private static void ReadFile( IEnumerable<string> paths ) {

			foreach( string path in paths ) {

				try {
					FlagFileReader.ReadAllText( path );

				} catch( FileNotFoundException ) {

				} catch( Exception e ) {

					Console.Error.WriteLine( e );
					return;
				}
			}
		}
	}
}
