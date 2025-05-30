FROM jenkins/agent:latest-jdk17

USER root

# Install dependencies and tools
RUN apt-get update && apt-get install -y \
    curl \
    wget \
    git \
    unzip \
    apt-transport-https \
    gnupg \
    ca-certificates \
    build-essential \
    && rm -rf /var/lib/apt/lists/*

# Install Node.js (latest LTS or current)
RUN curl -fsSL https://deb.nodesource.com/setup_current.x | bash - \
    && apt-get install -y nodejs \
    && npm install -g npm@latest

# Install Microsoft package signing key and repo for .NET 9 SDK
RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb

# Install .NET 9 SDK
RUN apt-get update && apt-get install -y dotnet-sdk-9.0

RUN apt-get update && apt-get install -y lsb-release

# Docker CLI
RUN curl -fsSL https://download.docker.com/linux/debian/gpg | gpg --dearmor -o /usr/share/keyrings/docker-archive-keyring.gpg \
    && echo "deb [arch=amd64 signed-by=/usr/share/keyrings/docker-archive-keyring.gpg] https://download.docker.com/linux/debian $(lsb_release -cs) stable" > /etc/apt/sources.list.d/docker.list \
    && apt-get update \
    && apt-get install -y docker-ce-cli

# Verify installations (optional)
RUN dotnet --version && node --version && npm --version

USER root
