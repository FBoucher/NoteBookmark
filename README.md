# Note Bookmark

![GitHub Release](https://img.shields.io/github/v/release/fboucher/NoteBookmark)  ![.NET](https://img.shields.io/badge/9.0-512BD4?logo=dotnet&logoColor=fff)  [![.github/workflows/azure-dev.yml](https://github.com/FBoucher/NoteBookmark/actions/workflows/azure-dev.yml/badge.svg)](https://github.com/FBoucher/NoteBookmark/actions/workflows/azure-dev.yml) 


I use this project mostly everyday. I build it to help me collecting my thoughts about articles, and blob posts I read during the week and then aggregate them in a #ReadingNotes blog post. You can find those post on my blog [here](https://frankysnotes.com).

NoteBookmark is composed of three main sections:

- **Post**: where you can manage a posts "to read", and add notes to them.
- **Generate Summary**: where you can generate a summary of the posts you read.
- **Summaries**: where you can see all the summaries you generated.

![Slide show of all NoteBookmark Screens](gh/images/NoteBookmark-Tour_hd.gif)

## How to deploy Your own NoteBookmark

### Get the code on your machine

- Fork this repository to your account.
- Clone the repository to your local machine.


### Deploy the solution (5 mins)

Using Azure Developer CLI let's initialize your environment. In a terminal, at the root of the project, run the following command. When ask give it a name (ex: NoteBookmark-dev).

```bash
azd init
```

Now let's deploy the solution. Run the following command in the terminal. You will have to select your Azure subscription where you want to deploy the solution, and a location (ex: eastus).

```bash
azd up
```

It should take around five minutes to deploy the solution. Once it's done, you will see the URL for **Deploying service blazor-app**.

### Secure the App in a few clicks

The app is now deployed, but it's not secure. Navigate to the Azure Portal, and find the Resource Group you just deployed (ex: rg-notebookmark-dev). In this resource group, open the Container App **Container App**. From the left menu, select **Authentication** and click the **Add identity provider**.

You can choose between multiple providers, I like to use Microsoft since it's deploy in Azure and I'm already logged in. If Microsoft is choose, select the recomended **Client secret expiration** (ex: 180 days). You can keep all the other default settings. Click **Add**.

Next time you will navigate to the app, you will be prompt a to login with your Microsoft account. The first time you will have a **Permissions requested** screen, click **Accept**.

Voila! Your app is now secure.

## Contributing

Your contributions are welcome! Take a look at [CONTRIBUTING](/CONTRIBUTING.md) for details.