/*
 * Jenkins Declarative Pipeline for Full Stack Application CI/CD
 * 
 * Author: Richard Benny - 22825546
 * 
 * This pipeline automates the continuous integration and delivery process for a full stack application
 * consisting of a .NET backend (API) and a Node.js frontend (UI). It is designed to run on a Jenkins agent
 * labeled 'docker-agent-docker9-node' and uses Docker for building and pushing images.
 * 
 * Key Features:
 * - Polls the SCM every 5 minutes for changes.
 * - Runs linting and unit tests for the frontend when changes are detected in the Frontend directory.
 * - Restores, builds, and tests the backend when changes are detected in the API directory.
 * - Builds production-ready frontend and backend artifacts.
 * - Archives build artifacts for both frontend and backend.
 * - Builds and pushes Docker images for both frontend and backend to Docker Hub, tagging with both the latest
 *   Git commit SHA and 'latest'.
 * - Uses Jenkins credentials for secure Docker Hub authentication.
 * - Logs out from Docker after the pipeline completes.
 * - Provides notifications on pipeline success or failure.
 */

pipeline {
    agent {
        node {
            label 'docker-agent-docker9-node'
        }
    }

    environment {
        DOCKER_CREDENTIALS = credentials('docker_hub')
    }

    triggers {
            pollSCM '*/5 * * * *'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Frontend Lint & Test') {
            when {
                changeset '**/Frontend/**'
            }
            steps {
                dir('Frontend/UI/') {
                    echo 'Installing frontend dependencies...'
                    sh 'npm ci'

                    echo 'Linting frontend...'
                    sh 'npm run lint'

                    echo 'Running frontend unit tests...'
                    sh 'npm run test'
                }
            }
        }

        stage('Backend Test') {
            when {
                changeset '**/API/**'
            }
            steps {
                dir('API/TDD') {
                    echo 'Restoring .NET dependencies...'
                    sh 'dotnet restore'
                    echo 'Building backend...'
                    sh 'dotnet build --no-restore'
                    echo 'Running backend tests...'
                    sh 'dotnet test --no-build'
                }
            }
        }

        stage('Build Frontend') {
            when {
                changeset '**/Frontend/**'
            }
            steps {
                dir('Frontend/UI/') {
                    echo 'Building frontend...'
                    sh 'npm run build:ssr'
                }
            }
        }

        stage('Build Backend') {
            when {
                changeset '**/API/**'
            }
            steps {
                dir('API/TDD') {
                    echo 'Rebuilding backend for deployment...'
                    sh 'dotnet publish -c Release -o ./publish'
                }
            }
        }

                stage('Archive Artifacts') {
                    when {
                        anyOf {
                            changeset '**/API/**'
                            changeset '**/Frontend/**'
                        }
                    }
                    steps {
                        dir('API/TDD') {
                            archiveArtifacts artifacts: 'publish/**', allowEmptyArchive: true
                        }
                        dir('Frontend/UI/') {
                            archiveArtifacts artifacts: 'dist/**', allowEmptyArchive: true
                        }
                    }
                }

        stage('Build & Push Backend Image') {
            when {
                changeset '**/API/**'
            }
            steps {
                script {
                    def apiImage = "richardbenny/devops-tca-api"
                    def gitSha = sh(script: 'git rev-parse --short HEAD', returnStdout: true).trim()
                    def versionTag = "${apiImage}:${gitSha}"
                    def latestTag = "${apiImage}:latest"

                    dir('API/TDD') {
                        sh "docker build -t ${versionTag} -t ${latestTag} ."
                        sh 'echo "$DOCKER_CREDENTIALS_PSW" | docker login -u "$DOCKER_CREDENTIALS_USR" --password-stdin'
                        sh "docker push ${versionTag}"
                        sh "docker push ${latestTag}"
                    }
                }
            }
        }
        stage('Build & Push Frontend Image') {
            when {
                changeset '**/Frontend/**'
            }
            steps {
                script {
                    def feImage = "richardbenny/devops-tca-frontend"
                    def gitSha = sh(script: 'git rev-parse --short HEAD', returnStdout: true).trim()
                    def versionTag = "${feImage}:${gitSha}"
                    def latestTag = "${feImage}:latest"

                    dir('Frontend/UI') {
                        sh "docker build -t ${versionTag} -t ${latestTag} ."
                        sh "docker push ${versionTag}"
                        sh "docker push ${latestTag}"
                    }
                }
            }
        }


    }

    post {
        always {
            sh 'docker logout'
        }
        success {
            echo 'CI pipeline passed'
        }
        failure {
            echo ' CI pipeline failed'
        }
    }
}
