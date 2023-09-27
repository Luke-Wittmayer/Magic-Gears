# Magic-Gears

## Workflow

We'll be using a workflow called GitFlow to manage our work. Here's a [video](https://www.youtube.com/watch?v=1SXpE08hvGs) that explains how it works. 
So if you want to add something you will:

* Make a feature branch based on the develop branch.
* Work on feature branch, make sure it works.
* Once ready, make a pull request based on develop branch. One person will have to review the code. Once it has been reviewed whoever asked for the pull request will merge it back to the develop branch.
* Finally delete the feature branch.

To update the main branch:

* Make a release branch based on the develop branch.
* Fix any small bugs.
* Merge these bug fixes onto the develop branch with a pull request.
* Merge release branch onto the main branch with a pull request.
* Finally delete release branch.

Try to not make huge changes to the develop branch. Make small incremental changes so that it's easier to merge and review the code.
