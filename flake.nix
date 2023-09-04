{
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/master";
    flake-utils.url = "github:numtide/flake-utils";
  };
  outputs = { self, nixpkgs, flake-utils, ... }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs {
          inherit system;
        };
      in
      with pkgs;
      {
        devShells.default = mkShell rec {
          dotnet = (with dotnetCorePackages; combinePackages [ sdk_6_0 ]);
          packages = [ dotnet ncurses ];
          DOTNET_ROOT = "${dotnet}";
          LD_LIBRARY_PATH = lib.makeLibraryPath [ ncurses ];
        };
      });
}
