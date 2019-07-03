"Exporting resources to file. This can take a really long time. A beep will play when it is finished."

if([System.IO.File]::Exists(".\FactoryGame-WindowsNoEditor.pak"))
{
	#command that does not save to file, instead printing it to the console
	#strings -n 30 .\FactoryGame-WindowsNoEditor.pak | Select-String -Pattern "^BlueprintGeneratedClass'/Game/FactoryGame/Resource" | % { "$_".replace("BlueprintGeneratedClass'", "") } | % { "$_".replace("'", "")} 

	strings -n 30 .\FactoryGame-WindowsNoEditor.pak | Select-String -Pattern "^BlueprintGeneratedClass'/Game/FactoryGame/Resource" | % { "$_".replace("BlueprintGeneratedClass'", "") } | % { "$_".replace("'", "")} | Add-Content .\Resources.txt
	[console]::beep(500,125)
	"Done."
}
else
{
	Write-Host "Couldn't find FactoryGame-WindowsNoEditor.pak\nAre you sure you copied this script into your \SatisfactoryEarlyAccess\FactoryGame\Content\Paks folder?" -ForegroundColor Red
}

pause