

			/*******************************/

				  Glass.Mapper.Sc V5

			/*******************************/

POST INSTALL (Glass.Mapper.Sc.xx package):

If you are using package reference, you will need to include the files from the 'App_Start' and 'App_Config' 
folders found at '\obj\{Build-Configuration}\NuGet\{Guid}\Glass.Mapper.Sc.xx\{Version}' in your solution if they are
not already present. 

You will also be required to copy the Glass.Mapper.Sc.config file to your solution:
https://github.com/mikeedwards83/Glass.Mapper/blob/master/Source/Glass.Mapper.Sc/App_Config/Include/Glass/Glass.Mapper.Sc.config

  _______ _    _          _   _ _  __ __     ______  _    _     _ 
 |__   __| |  | |   /\   | \ | | |/ / \ \   / / __ \| |  | |   | |
    | |  | |__| |  /  \  |  \| | ' /   \ \_/ / |  | | |  | |   | |
    | |  |  __  | / /\ \ | . ` |  <     \   /| |  | | |  | |   | |
    | |  | |  | |/ ____ \| |\  | . \     | | | |__| | |__| |   |_|
    |_|  |_|  |_/_/    \_\_| \_|_|\_\    |_|  \____/ \____/    (_)


Glass.Mapper.Sc is made possible by the generous donations of our Patreon backers! Personally from myself a massive
THANK YOU to all of you!! - Mike

To find out more about our backers visit https://www.glass.lu/rockstars.

A special thank you to the following supporters of the Glass.Mapper.Sc project:

                            Swissworx

							Dataweavers

Anis Chohan					Amir Setoudeh				Chaturanga Ranatunga 
Ishraq Al Fataftah			Dylan Young					Jason Wilkerson 
Mohannad Alhasasneh			Robbert Hock				Steve McGill
Eric Stafford				Matt Fletcher				Floris Briolas


If you would like to add your name to this amazing list of people visit our Patreon page https://www.patreon.com/glassmappersc

/******* GETTING STARTED ********/

To register Glass.Mapper.Sc in your application during service configuration call AddGlassMapper:

using  Glass.Mapper.Sc;

public void ConfigureServices(IServiceCollection services)
{
        services.AddGlassMapper();
}

If you are upgrading from a legacy Glass.Mapper.Sc setup you can continue to use the files in the App_Start folder.

/****** LEARN MORE ABOUT GLASS ******/

Learn how to use the framework by getting Glass.Mapper.Sc V5 Training here http://www.glass.lu/Training.

For help and support visit https://www.glass.lu/support.

-----------------------------------------------------------------------------------------------------

For more information on how to use the framework please visit http://www.glass.lu/Mapper

Glass.Mapper.Sc is distributed under the Apache License 2.0 https://github.com/mikeedwards83/Glass.Mapper/blob/master/License.txt

We automatically generate a website contain help information which you can read at http://docs.glass.lu

We have a great community who will help answer your questions at http://stackoverflow.com/ but if you need more 
specialist support please contact us at hello@glass.lu where we will be able to offer our consultancy services.



