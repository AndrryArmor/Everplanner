<template>
  <form>
    <div class="container-fluid mt-3 workers">
      <h4 class="ms-5">Співробітники</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto table-col">
          <table class="table m-0">
            <thead>
              <th></th>
              <th>Id</th>
              <th>ПІБ</th>
              <th>Зарплата</th>
              <th>Швидкість розробки</th>
            </thead>
            <tbody>
              <tr>
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>1</td>
                <td>Ачілов А. В.</td>
                <td>1000</td>
                <td>15 {{ developmentVelocityMetric }}</td>
              </tr>
              <tr>
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>
                  2
                </td>
                <td>
                  <input type="text" class="form-control form-control-sm" />
                </td>
                <td>
                  <div class="input-group">
                    <input type="number" min="0" class="form-control form-control-sm" />
                    <span class="input-group-text">{{ dollarSign }}</span>
                  </div>
                </td>
                <td>
                  <div class="input-group">
                    <input type="number" min="0" class="form-control form-control-sm" />
                    <span class="input-group-text">{{ developmentVelocityMetric }}</span>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="container-fluid mt-3 tasks">
      <h4 class="ms-5">Задачі</h4>
      <div class="row overflow-x-auto">
        <div class="col-auto table-col">
          <table class="table m-0">
            <thead>
              <th></th>
              <th>Id</th>
              <th>Назва задачі</th>
              <th>Складність</th>
              <th>Залежить від</th>
              <th>Доступні співробітники для виконання</th>
            </thead>
            <tbody>
              <tr>
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>1</td>
                <td>Створити діаграму</td>
                <td>8 {{ storyPointsSign }}</td>
                <td></td>
                <td></td>
              </tr>
              <tr>
                <td>
                  <button type="button" class="btn btn-outline-danger p-1 trashbin">
                    <i class="bi bi-trash"></i>
                  </button>
                </td>
                <td>
                  2
                </td>
                <td>
                  <input type="text" class="form-control form-control-sm" />
                </td>
                <td>
                  <div class="input-group">
                    <input type="number" min="0" max="100" class="form-control form-control-sm" />
                    <span class="input-group-text">{{ storyPointsSign }}</span>
                  </div>
                </td>
                <td>
                  <div class="container-fluid">
                    <div class="row gap-1">
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 1
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 2
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 3
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 4
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Задача 5
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto dropdown d-inline-block p-0">
                        <button class="btn btn-sm btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                          Додати...
                        </button>
                        <ul class="dropdown-menu">
                          <li><a class="dropdown-item" href="#">Задача 2</a></li>
                          <li><a class="dropdown-item" href="#">Задача 3</a></li>
                          <li><a class="dropdown-item" href="#">Задача 4</a></li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </td>
                <td>
                  <div class="container-fluid">
                    <div class="row gap-1">
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Ачілов А. В.
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Ковальчук Б. Г
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Сніжний М. Г.
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block border-primary text-primary inline-item">
                        Струменець Л. Д.
                        <button type="button" class="btn-close"></button>
                      </div>
                      <div class="col-auto d-inline-block p-0 dropdown">
                        <button class="btn btn-sm btn-primary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                          Додати...
                        </button>
                        <ul class="col-auto dropdown-menu">
                          <li><a class="dropdown-item" href="#">Ачілов А. В.</a></li>
                          <li><a class="dropdown-item" href="#">Сніжний М. Г.</a></li>
                          <li><a class="dropdown-item" href="#">Струменець Л. Д.</a></li>
                        </ul>
                      </div>
                    </div>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <button type="button" class="btn btn-lg btn-primary m-2">Cпланувати проєкт</button>
  </form>
</template>

<script setup>
const dollarSign = '$';
const storyPointsSign = "SP";
const developmentVelocityMetric = `${storyPointsSign}/тиж.`;
</script>

<style scoped lang="scss">
@import "../assets/styles.scss";

.table-col {
  width: fit-content;
}

.table {

  th {
    text-align: center;
  }

  vertical-align: middle;
  width: fit-content;

  td button.trashbin {
    border-width: 0px;
    margin: -0.25em -0.25em -0.25em -0.25em;
  }

  .btn-close {
    font-size: 0.75em;
  }
}

$td-trashbin-width: 30px;
$td-id-width: 50px;
$td-badges-width: 500px;
$td-worker-name-width: 300px;
$td-salary-width: 150px;
$td-task-name-width: 300px;
$td-velocity-width: 200px;
$td-complexity-width: 125px;

.workers .table {

  th:nth-child(1),
  td:nth-child(1) {
    width: $td-trashbin-width;
    min-width: $td-trashbin-width;
    max-width: $td-trashbin-width;
    text-align: center;
  }

  th:nth-child(2),
  td:nth-child(2) {
    width: $td-id-width;
    min-width: $td-id-width;
    max-width: $td-id-width;
    text-align: center;
  }

  th:nth-child(3),
  td:nth-child(3) {
    width: $td-worker-name-width;
    min-width: $td-worker-name-width;
    max-width: $td-worker-name-width;
  }

  th:nth-child(4),
  td:nth-child(4) {
    width: $td-salary-width;
    min-width: $td-salary-width;
    max-width: $td-salary-width;
    text-align: center;
  }

  th:nth-child(5),
  td:nth-child(5) {
    width: $td-velocity-width;
    min-width: $td-velocity-width;
    max-width: $td-velocity-width;
    text-align: center;
  }
}

.tasks .table {

  th:nth-child(1),
  td:nth-child(1) {
    width: $td-trashbin-width;
    min-width: $td-trashbin-width;
    max-width: $td-trashbin-width;
    text-align: center;
  }

  th:nth-child(2),
  td:nth-child(2) {
    width: $td-id-width;
    min-width: $td-id-width;
    max-width: $td-id-width;
    text-align: center;
  }

  th:nth-child(3),
  td:nth-child(3) {
    width: $td-task-name-width;
    min-width: $td-task-name-width;
    max-width: $td-task-name-width;
  }

  th:nth-child(4),
  td:nth-child(4) {
    width: $td-complexity-width;
    min-width: $td-complexity-width;
    max-width: $td-complexity-width;
    text-align: center;
  }

  th:nth-child(5),
  td:nth-child(5) {
    width: $td-badges-width;
    min-width: $td-badges-width;
    max-width: $td-badges-width;
  }

  th:nth-child(6),
  td:nth-child(6) {
    width: $td-badges-width;
    min-width: $td-badges-width;
    max-width: $td-badges-width;
  }
}

.inline-item {
  padding: 0.25rem 0.5rem;
  border: 1px solid;
  border-radius: 0.25rem;
  font-size: 0.875rem;
}

.dropdown {
  position: static;
}
</style>
