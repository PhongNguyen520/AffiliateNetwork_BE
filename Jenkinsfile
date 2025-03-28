pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'nguyenphong203/affiliate_networking'
        DOCKER_TAG = "${env.BUILD_ID}"
    }

    stages {
        stage('Checkout') {
            steps {
                checkout([
                    $class: 'GitSCM',
                    branches: [[name: '*/main']],  // Explicitly use main branch
                    extensions: [],
                    userRemoteConfigs: [[
                        url: 'https://github.com/PhongNguyen520/AffiliateNetwork_BE.git'
                    ]]
                ])
            }
        }

        stage('Build') {
            steps {
                script {
                    // Verify Docker installation
                    sh 'docker --version'
                    
                    // Build Docker image with tag
                    sh "docker build -t ${DOCKER_IMAGE}:${DOCKER_TAG} ."
                }
            }
        }

        stage('Push Docker') {
            steps {
                withDockerRegistry(
                    credentialsId: 'docker-hub', 
                    url: 'https://index.docker.io/v1/'
                ) {
                    script {
                        // Push versioned image
                        sh "docker push ${DOCKER_IMAGE}:${DOCKER_TAG}"
                        
                        // Also push as latest
                        sh "docker tag ${DOCKER_IMAGE}:${DOCKER_TAG} ${DOCKER_IMAGE}:latest"
                        sh "docker push ${DOCKER_IMAGE}:latest"
                    }
                }
            }
        }
    }

    post {
        success {
            echo 'Pipeline completed successfully!'
            echo "Docker image pushed as: ${DOCKER_IMAGE}:${DOCKER_TAG}"
        }
        failure {
            echo 'Pipeline failed!'
        }
        always {
            // Clean up Docker images to save space
            sh 'docker system prune -f'
        }
    }
}