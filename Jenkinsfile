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

        stage('Files') {
            steps {
                echo 'Checking for changes in the repository...'
                sh 'pwd'
                sh 'ls -la'
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

        stage('Backend Lint & Test') {
            when {
                changeset '**/API/**'
            }
            steps {
                dir('API/TDD') {
                    echo 'Restoring .NET dependencies...'
                    sh 'dotnet restore'
                    echo 'Linting backend code (optional)...'
                    // sh 'dotnet format  --verify-no-changes'
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
            steps {
                dir('API/TDD') {
                    archiveArtifacts artifacts: 'publish/**', allowEmptyArchive: true
                }
                dir('Frontend/UI/') {
                    archiveArtifacts artifacts: 'dist/**', allowEmptyArchive: true
                }
            }
        }


        stage('Docker Build') {
            steps {
                sh 'docker build -t richardbenny/devops-tca:latest .'
            }
        }

        stage('Deploy to Docker') {
            steps {
                script {
                    def gitSha = sh(script: 'git rev-parse --short HEAD', returnStdout: true).trim()
                    def imageName = "richardbenny/devops-tca"
                    def versionTag = "${imageName}:${gitSha}"
                    def latestTag = "${imageName}:latest"

                    sh "docker build -t ${versionTag} ."
                    sh 'echo "$DOCKER_CREDENTIALS_PSW" | docker login -u "$DOCKER_CREDENTIALS_USR" --password-stdin'

                    sh "docker push ${versionTag}"
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
