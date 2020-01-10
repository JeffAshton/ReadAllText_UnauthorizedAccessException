using System;
using System.IO;
using System.Threading;

namespace FileWatcherMover {

	internal static class Program {

		internal static int Main() {

			string directory = Path.GetFullPath( "." );

			string sourcePath = Path.Combine( directory, "source.txt" );
			string configPath = Path.Combine( directory, "current.txt" );
			string previousPath = Path.Combine( directory, "previous.txt" );

			if( !File.Exists( sourcePath ) ) {

				Console.Error.WriteLine( "Source file not found: {0}", sourcePath );
				return 1;
			}
			
			File.Delete( previousPath );

			for(; ; ) {

				string tempPath = Path.GetTempFileName();
				File.Copy( sourcePath, tempPath, overwrite: true );

				retryPreviousMove:
				try {
					File.Move( configPath, previousPath );

				} catch( FileNotFoundException ) {

				} catch( IOException err ) when( IsFileLocked( err ) ) {
					goto retryPreviousMove;

				} catch( Exception err ) {
					Console.Error.WriteLine( "Failed to move current to previous: {0}", err );
				}

				retryConfigMove:
				try {
					File.Move( tempPath, configPath );

				} catch( IOException err ) when( IsFileLocked( err ) ) {
					goto retryConfigMove;

				} catch( Exception err ) {
					Console.Error.WriteLine( "Failed to move new current: {0}", err );
				}

				retryDelete:
				try {
					File.Delete( previousPath );

				} catch( IOException err ) when( IsFileLocked( err ) ) {
					goto retryDelete;

				} catch( Exception err ) {
					Console.Error.WriteLine( "Failed to delete previous: {0}", err );
				}

				Thread.Sleep( 1000 );
			}
		}


		private static bool IsFileLocked( IOException exception ) {
			// We cannot guarantee that these HResult values will be present on non-Windows OSes. However, this
			// logic is less important on other platforms, because in Unix-like OSes you can atomically replace a
			// file's contents (by creating a temporary file and then renaming it to overwrite the original file),
			// so FileDataSource will not try to read an incomplete update; that is not possibble in Windows.
			int errorCode = exception.HResult & 0xffff;
			switch( errorCode ) {
				case 0x20: // ERROR_SHARING_VIOLATION
				case 0x21: // ERROR_LOCK_VIOLATION
					return true;
				default:
					return false;
			}
		}
	}
}
