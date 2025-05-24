pipeline {
    agent {
        node {
            label 'docker-agent-docker9-node'
        }
    }

    triggers {
            pollSCM 'H/5 * * * *'
        }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }


        stage('Frontend Lint & Test') {
            // when {
            //     changeset "**/frontend/**"
            // }
            steps {
                dir('frontend') {
                    echo "Installing frontend dependencies..."
                    // sh 'npm ci'

                    echo "Linting frontend..."
                    // sh 'npm run lint'

                    echo "Running frontend unit tests..."
                    // sh 'npm test -- --watch=false --browsers=ChromeHeadless'
                }
            }
        }

        stage('Backend Lint & Test') {
            // when {
            //     changeset "**/API/**"
            // }
            steps {
                dir('API/EventManagement') {
                    sh 'pwd'

                    sh 'ls -ltra'

                    echo "Restoring .NET dependencies..."
                    sh 'cd EventManagement.Web && ls -ltra'
                    sh 'pwd'
                    sh 'dotnet restore EventManagement.Web.csproj'
                    sh 'cd ../..'
                    // sh 'dotnet restore --project EventManagement.Web/EventManagement.Web.csproj'

                    echo "Linting backend code (optional)..."
                    sh 'dotnet format  --verify-no-changes' // Optional for code formatting

                    echo "Building backend..."
                    sh 'dotnet build --project EventManagement.Web/EventManagement.Web.csproj --no-restore'

                    echo "Running backend tests..."
                    // sh 'dotnet test --project EventManagement.Web/EventManagement.Web.csproj --no-build'
                }
            }
        }

        stage('Build Frontend') {
            // when {
            //     changeset "**/frontend/**"
            // }
            steps {
                dir('frontend') {
                    echo "Building frontend..."
                    // sh 'npm run build'
                }
            }
        }

        stage('Build Backend') {
            // when {
            //     changeset "**/backend/**"
            // }
            steps {
                dir('API/EventManagement') {
                    echo "Rebuilding backend for deployment..."
                    sh 'dotnet publish -c Release -o ./publish'
                }
            }
        }

        // stage('Archive Artifacts') {
        //     steps {
        //         dir('backend') {
        //             archiveArtifacts artifacts: 'publish/**', allowEmptyArchive: true
        //         }
        //         dir('frontend') {
        //             archiveArtifacts artifacts: 'dist/**', allowEmptyArchive: true
        //         }
        //     }
        // }

        stage('Deploy') {
            steps {
                echo "Deploy here, using shell scripts or external jobs"
            }
        }
    }

    post {
        // always {
        //     junit '**/TestResults/*.xml' // Only if you export test results in XML
        // }
        success {
            echo 'CI pipeline passed'
        }
        failure {
            echo ' CI pipeline failed'
        }
    }
}
