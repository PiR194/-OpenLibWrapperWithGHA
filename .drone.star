def main(ctx):
  return {
    "kind": "pipeline",
    "type": "docker",
    "name": "OpenLibPipeLine",
    "steps": [
      {
        "name": "build",
        "image": "mcr.microsoft.com/dotnet/sdk:7.0",
        "commands": [
            "cd Sources/",
            "dotnet restore 	OpenLibraryWS_Wrapper.sln",
            "dotnet build 	OpenLibraryWS_Wrapper.sln -c Release --no-restore",
            "dotnet publish 	OpenLibraryWS_Wrapper.sln -c Release --no-restore -o CI_PROJECT_DIR/build/release",
            "dotnet tool install --version 6.5.0 Swashbuckle.AspNetCore.Cli",
            "dotnet new tool-manifest",
            "dotnet swagger tofile --output /docs/swagger.json CI_PROJECT_DIR/build/release/OpenLibraryWS_WrapperC1.dll OpenLibraryWS_WrapperC1.xml"
        ]
      },
      {
        "name": "docker-build-and-push",
        "image": "plugins/docker",
        "settings": {
            "dockerfile":"docs/Dockerfile",
            "context": "Sources/",unmarshal
            "registry": "hub.codefirst.iut.uca.fr",
            "repo": "hub.codefirst.iut.uca.fr/pierre.ferreira/openlibraryws_wrapperc1",
            "username": {
                "from_secret":"SECRET_REGISTRY_USERNAME"
            },
            "password": {
                "from_secret":"SECRET_REGISTRY_PASSWORD"
            }
        }
      }
    ]
  }

