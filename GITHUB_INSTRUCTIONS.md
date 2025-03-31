# GitHub Repository Setup Instructions

Follow these steps to push your Simple File Renamer project to GitHub:

## 1. Create a new repository on GitHub

1. Go to [GitHub](https://github.com/) and sign in to your account
2. Click the "+" icon in the top-right corner and select "New repository"
3. Enter "SimpleFileRenamer" as the repository name
4. Add a description: "A Windows desktop application for batch file renaming with advanced pattern-based rules and Windows Explorer integration"
5. Choose "Public" or "Private" visibility as desired
6. Do NOT initialize the repository with a README, .gitignore, or license (we'll push these from our existing code)
7. Click "Create repository"

## 2. Connect your local repository to GitHub

After creating the repository, GitHub will show you commands to push an existing repository. Use the following commands in your terminal or command prompt:

```bash
# Make sure you're in the project root directory
cd /path/to/SimpleFileRenamer

# Add the GitHub repository as a remote called "origin"
git remote add origin https://github.com/yourusername/SimpleFileRenamer.git

# Push your local main branch to GitHub
git push -u origin main
```

Replace `yourusername` with your actual GitHub username.

## 3. Verify the upload

1. Go to your GitHub repository at `https://github.com/yourusername/SimpleFileRenamer`
2. Confirm that all your files and folders are present in the repository

## 4. Set up GitHub Pages (Optional)

If you want to create a project website:

1. Go to your repository on GitHub
2. Click "Settings" > "Pages"
3. Under "Source", select "main" and "/docs" folder (or create a docs folder in your repository)
4. Click "Save"
5. Wait a few minutes for your site to be published

## 5. Set up releases

Once you've pushed your code:

1. Go to your repository on GitHub
2. Click "Releases" in the right sidebar
3. Click "Create a new release"
4. Enter a tag version (e.g., "v1.0.0")
5. Enter a title for the release (e.g., "Initial Release")
6. Write a description of your release
7. Attach your built binaries (ZIP files from the release folder)
8. Click "Publish release"

This will make your application available for download from GitHub.

## 6. Update README.md links

The README.md file has been prepared with placeholders that need to be updated after you create your GitHub repository:

1. Replace "YOUR-USERNAME" in the clone URL with your actual GitHub username
2. After you create your first release, update the installation instructions to point to the actual releases page

Example changes:
- Change `https://github.com/YOUR-USERNAME/SimpleFileRenamer.git` to your actual repository URL
- Update "Download from the Releases page (will be available after first GitHub release)" with direct links to your releases page