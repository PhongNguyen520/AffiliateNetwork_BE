pipeline{
    agent any

    stages{
        stage('Build') {
            steps {
            echo 'Verifying Docker installation'
                sh 'docker --version' 
                git 'https://github.com/PhongNguyen520/Jenkins.git'
            }
        }

        // stage('PushDocker') {
        //     steps {
        //         // Use the default Docker Hub registry URL (optional)
        //         withDockerRegistry(credentialsId: 'docker-hub', url: 'https://index.docker.io/v1/') {
        //             sh 'docker build -t nguyenphong203/aspnetcoretest:v3 -f SwaggerTest/Dockerfile .'
        //             sh 'docker push nguyenphong203/aspnetcoretest:v3'
        //         }
        //     }
        // }
    }
}
