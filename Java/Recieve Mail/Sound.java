// Zachary Tennant

import java.io.*;
import java.net.*;
import javax.sound.sampled.*;

public class Sound
{
	static Clip audioClip;
	AudioInputStream audioInputStream;
	URL url;
//===============================================
	public static void main(String[] aaa)
	{
		new Sound().makeASound();

		try
		{
			Thread.sleep(2000);
		}
		catch(Exception e)
		{
			System.out.println("Sleep interrupted exception occurred.");
		}

		System.out.println("Program ending...");
	}
//===============================================
	void makeASound()
	{
		try
		{
			url = this.getClass().getClassLoader().getResource("Kawai-K1r-Aah-C4.wav");

			audioInputStream = AudioSystem.getAudioInputStream(url);
			audioClip = AudioSystem.getClip();
			audioClip.open(audioInputStream);
			audioClip.start();
		}
		catch(UnsupportedAudioFileException uafe)
		{
			System.out.println("Unsupported File");
		}
		catch(IOException ioe)
		{
			System.out.println("IO exception");
		}
		catch(LineUnavailableException lue)
		{
			System.out.println("Line unavailable exception");
		}
		catch(NullPointerException npe)
		{
			System.out.println("Null pointer exception");
		}
	}
} // end of SoundSimple class