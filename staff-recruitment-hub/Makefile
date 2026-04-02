MAIN_PROJECT = src/SRB_WebPortal
MODEL_PROJECT = src/SRB_ViewModel
PADDLE_PROJECT = src/SRB_Paddle

RAW_ARGS = $(filter-out $@,$(MAKECMDGOALS))
ARGS = $(if $(RAW_ARGS), $(RAW_ARGS), InitialEntity)

.PHONY: run dev build migrate updateDB clean restore

all: run

run:
	dotnet run --project $(MAIN_PROJECT) --no-https
dev:
	dotnet watch run --project $(MAIN_PROJECT) --no-https
mo:
	py ${PADDLE_PROJECT}/main
# dev:
# 	dotnet watch run --project $(MAIN_PROJECT) --no-https --no-hot-reload
devUI:
	npx @tailwindcss/cli \
		-i $(MAIN_PROJECT)/wwwroot/css/input.css \
		-o $(MAIN_PROJECT)/wwwroot/css/output.css \
		--content "$(MAIN_PROJECT)/**/*.{.cshtml,html,js}" \
		--watch
build:
	dotnet build $(MAIN_PROJECT) --no-restore
migrate:
	dotnet ef migrations add ${ARGS} --project ${MODEL_PROJECT} --startup-project $(MAIN_PROJECT) -v
updateDB:
	dotnet ef database update --project $(MODEL_PROJECT) --startup-project $(MAIN_PROJECT) -v
clean:
	dotnet clean
	find . -type d -name "obj" -exec rm -rf {} +
	find . -type d -name "bin" -exec rm -rf {} +
	find . -type d -name "node_modules" -exec rm -rf {} +
restore:
	dotnet restore
	npm install
	cd src/SRB_WebPortal && npm install

%:
	@:
