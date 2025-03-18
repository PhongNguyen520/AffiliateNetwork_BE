pipeline{
     agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:6.0'
            args '-v /var/run/docker.sock:/var/run/docker.sock' // Bind Docker socket
        }
    }
    environment {
        DOCKER_HUB_CREDENTIALS = credentials('docker-hub') // Sử dụng credentials Docker Hub
        DOCKER_IMAGE = 'nguyenphong203/affiliate_networking' // Tên image trên Docker Hub
        DOCKER_TAG = 'latest' // Tag của image
    }

    stages {
        // Stage 1: Checkout code từ repository
        stage('Checkout') {
            steps {
                echo 'Checking out code from repository...'
                git branch: 'main', url: 'https://github.com/PhongNguyen520/AffiliateNetwork_BE.git'
            }
        }

        // Stage 2: Build ứng dụng ASP.NET Web API
        stage('Build Application') {
            steps {
                echo 'Building ASP.NET Web API...'
                sh "dotnet build --configuration Release"
            }
        }

        // Stage 3: Chạy unit tests
        stage('Unit Tests') {
            steps {
                echo 'Running unit tests...'
                sh "dotnet test"
            }
        }

        // Stage 4: Publish ứng dụng
        stage('Publish Application') {
            steps {
                echo 'Publishing ASP.NET Web API...'
                sh "dotnet publish --configuration Release --output ./publish-output"
            }
        }

        // Stage 5: Build Docker image
        stage('Build Docker Image') {
            steps {
                echo 'Building Docker image...'
                sh """
                    docker build -t ${DOCKER_IMAGE}:${DOCKER_TAG} .
                """
            }
        }

        // Stage 6: Push Docker image lên Docker Hub
        stage('Push Docker Image To Docker Hub') {
            steps {
                echo 'Pushing Docker image to Docker Hub...'
                withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
                    sh """
                        docker push ${DOCKER_IMAGE}:${DOCKER_TAG}
                    """
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully!'
            slackSend channel: '#ci-cd', message: 'Pipeline succeeded!'
        }
        failure {
            echo 'Pipeline failed!'
            slackSend channel: '#ci-cd', message: 'Pipeline failed! Please check Jenkins logs.'
        }
    }
}
