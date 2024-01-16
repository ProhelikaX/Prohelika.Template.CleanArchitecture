#!/bin/zsh

PROJECT_NAME=$(echo "Prohelika.Template.CleanArchitecture" | tr '[:upper:]' '[:lower:]' | sed 's/\./_/g')

export PROJECT_NAME

echo "Choose environment:"
echo "1. Development"
echo "2. Production"

read -r env

if [ "$env" = "1" ]; then
  export DOCKER_COMPOSE_FILE=docker-compose.yml
else
  export DOCKER_COMPOSE_FILE=docker-compose-prod.yml
fi

echo "Choose an action:"
echo "1. Build"
echo "2. Run"
echo "3. Stop"

read -r action

if [ "$action" = "1" ]; then
  echo "Pull latest changes? (y/n)"
  
  read -r pull
  
  if [ "$pull" = "y" ]; then
    git pull
  fi 
  
  docker-compose -p "$PROJECT_NAME" -f $DOCKER_COMPOSE_FILE build
  
  echo "Do you want to run the containers? (y/n)"
  
  read -r run
  
  if [ "$run" = "y" ]; then
    docker-compose -p "$PROJECT_NAME" -f $DOCKER_COMPOSE_FILE up -d
  fi
  
elif [ "$action" = "2" ]; then
  docker-compose -p "$PROJECT_NAME" -f $DOCKER_COMPOSE_FILE up -d
elif [ "$action" = "3" ]; then
  docker-compose -p "$PROJECT_NAME" -f $DOCKER_COMPOSE_FILE down
fi 